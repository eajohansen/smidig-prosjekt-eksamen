using System.Collections;
using System.Security.Claims;
using agile_dev.Models;
using agile_dev.Repo;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace agile_dev.Service;

public class UserService {
    private readonly InitContext _dbCon;

    public UserService(InitContext context) {
        _dbCon = context;
    }

    /*
        Using Async methods to avoid blocking the main thread.
        This is important because the main thread is responsible for handling incoming requests.

        Exceptions are thrown when an error occurs.
        This is important because it allows the caller to handle the error (Controller)

        Task<> is used because we are using async methods.
    */

    #region GET

    public async Task<ICollection<User>> FetchAllUsers() {
        try {
            ICollection<User> foundUsers = await _dbCon.User.ToListAsync();
            ICollection<User> newUsers = AddRelationToUser(foundUsers.ToList()).Result;

            return newUsers;
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while fetching users.", exception);
        }
    }

    public async Task<User?> FetchUserById(int id) {
        try {
            User? user = await _dbCon.User.FindAsync(id);
            if (user != null) {
                List<User> foundUser = [user];
                foundUser = AddRelationToUser(foundUser).Result;
                return foundUser[0];
            }
            else {
                return user;
            }
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while fetching user.", exception);
        }
    }

    public async Task<User?> FetchUserByEmail(string email) {
        try {
            User? user = await _dbCon.User.FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while fetching user by email.", exception);
        }
    }

    #endregion

    #region POST

    public async Task<string> AddUserToDatabase(User user) {
        try {
            if (!_dbCon.User.AnyAsync().Result) {
                user.Admin = true;
            }

            User? userExists = await FetchUserByEmail(user.Email);

            if (userExists != null) {
                return "User already exists.";
            }
            
            List<Allergy> allergies = new List<Allergy>();
            if(user.Allergies != null && user.Allergies.Count > 0) {
               allergies = user.Allergies.ToList();
            }
            User newUser = user;
            newUser.Allergies = new List<Allergy>(allergies);
            
            await _dbCon.User.AddAsync(newUser); 
            await _dbCon.SaveChangesAsync();
            return newUser.Email + " has been added to the database.";
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while adding user to database.", exception);
        }
    }

    public async Task<bool> AddUserAsOrganizer(int loggedInUserId, User user, int organizationId) {
        try {
            User? foundUser = await _dbCon.User.FindAsync(loggedInUserId);
            Organization? foundOrganization = await _dbCon.Organization.FindAsync(organizationId);

            if (foundUser == null || foundOrganization == null) {
                return false;
            }
            
            bool isUserAdmin = IsUserAdmin(foundUser).Result;
            bool isLoggedInUserOrganizer = IsUserOrganizerForOrganization(foundUser, foundOrganization).Result;
            bool isUserAlreadyOrganizerForOrganization = IsUserOrganizerForOrganization(user, foundOrganization).Result;

            if (!isUserAlreadyOrganizerForOrganization && (!isLoggedInUserOrganizer || !isUserAdmin)) {
                return false;
            }

            Organizer newOrganizer = new (user.UserId, organizationId);
            await _dbCon.Organizer.AddAsync(newOrganizer);
            await _dbCon.SaveChangesAsync();
            return true;
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while adding user as organizer.", exception);
        }
    }

    public async Task<bool> AddUserAsFollower(User user, int organizationId) {
        try {
            Organization? foundOrganization = await _dbCon.Organization.FindAsync(organizationId);

            if (foundOrganization == null || !IsUserFollowingOrganization(user, foundOrganization).Result) {
                return false;
            }

            Follower newFollower = new (user.UserId, organizationId);
            await _dbCon.Follower.AddAsync(newFollower);
            await _dbCon.SaveChangesAsync();
            return true;
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while adding user as follower.", exception);
        }
    }

    public async Task<bool> AddUserToEvent(User user, int eventId) {
        try {
            Event? foundEvent = await _dbCon.Event.FindAsync(eventId);

            if (foundEvent != null && !IsUserAttendingEvent(user, foundEvent).Result) {
                return false;
            }

            UserEvent newUserEvent = new (user.UserId, eventId) {
                Used = false
            };
            
            await _dbCon.UserEvent.AddAsync(newUserEvent);
            await _dbCon.SaveChangesAsync();
            return true;
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while adding user to event.", exception);
        }
    }

    public async Task<bool> AddNoticeToUser(User user) {
        try {
            Notice newNotice = new (user.UserId) {
                Expire = DateTime.Now.AddDays(30)
            };

            await _dbCon.Notice.AddAsync(newNotice);
            await _dbCon.SaveChangesAsync();
            return true;
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while adding user to event.", exception);
        }
    }

    #endregion

    #region PUT

    public async Task<bool> UpdateUser(User user) {
        try {
            User? databaseUser = await FetchUserById(user.UserId);
            if (databaseUser == null) {
                return false;
            }

            // Check if databaseUser and user have the same autorization level
            if (user.Admin != databaseUser.Admin) {
                return false;
            }
            
            // Handle allergies

            // databaseUser.UserEvents = new List<UserEvent>();
            // databaseUser.Allergies = new List<Allergy>();
            // databaseUser.FollowOrganization = new List<Follower>();
            // databaseUser.OrganizerOrganization = new List<Organizer>();
            // databaseUser.Notices = new List<Notice>();
            
            if (databaseUser.Allergies.Count != 0) {
                foreach (Allergy allergy in databaseUser.Allergies) {
                    _dbCon.Allergy.Remove(allergy);
                }
            }
            
            if (user.Allergies.Count != 0) {
                foreach (Allergy allergy in user.Allergies) {
                    allergy.UserId = user.UserId;
                    await _dbCon.Allergy.AddAsync(allergy);
                }
            }
            
            databaseUser.Email = user.Email;
            databaseUser.Birthdate = user.Birthdate;
            databaseUser.ExtraInfo = user.ExtraInfo;
            databaseUser.FirstName = user.FirstName;
            databaseUser.LastName = user.LastName;
            
                _dbCon.User.Update(databaseUser);
                
            await _dbCon.SaveChangesAsync();
            
            return true;
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while updating user.", exception);
        }
    }

    public async Task<bool> MakeUserAdmin(User adminUser, int id) {
        try {
            bool isUserAdmin = IsUserAdmin(adminUser).Result;
            if (!isUserAdmin) {
                return false;
            }

            User? databaseUser = FetchUserById(id).Result;
            if (databaseUser == null) {
                return false;
            }

            databaseUser.Admin = true;

            // databaseUser.Role = "admin";
            await _dbCon.SaveChangesAsync();
            return true;
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while updating user.", exception);
        }
    }

    #endregion

    #region DELETE
    public async Task<bool> DeleteUser(User user) {
        try {
            
            _dbCon.User.Remove(user);
            await _dbCon.SaveChangesAsync();
            return true;
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while deleting user.", exception);
        }
    }

    #endregion

    #region MISCELLANEOUS

    public async Task<bool> IsUserAdmin(User user) {
        User? databaseAdminUser = await FetchUserById(user.UserId);
        return databaseAdminUser is { Admin: true };
    }

    public async Task<bool> IsUserOrganizerForOrganization(User user, Organization organization) {
        User? databaseUser = await FetchUserById(user.UserId);
        
        // Loops through all the organizations the user is an organizer for, and returns true if we find the id of the organization we are looking for
        return databaseUser != null && databaseUser.OrganizerOrganization.Any(organizations =>
            organizations.OrganizationId.Equals(organization.OrganizationId));
    }

    private async Task<bool> IsUserFollowingOrganization(User user, Organization organization) {
        User? databaseUser = await FetchUserById(user.UserId);
        
        // Loops through all the organizations the user is following, and returns true if we find the id of the organization we are looking for
        return databaseUser != null && databaseUser.FollowOrganization.Any(organizations =>
            organizations.OrganizationId.Equals(organization.OrganizationId));
    }

    private async Task<bool> IsUserAttendingEvent(User user, Event eEvent) {
        User? databaseUser = await FetchUserById(user.UserId);
        
        // Loops through all the events the user is attending, and returns true if we find the id of the event we are looking for
        return databaseUser != null &&
               databaseUser.UserEvents.Any(userEvent => userEvent.EventId.Equals(eEvent.EventId));
    }

    private async Task<List<User>> AddRelationToUser(List<User> users) {
        List<int> userIds = users.Select(user => user.UserId).ToList();

        List<User> newUsers = await _dbCon.User
            .Where(user => userIds.Contains(user.UserId))
            .Include(user => user.Allergies)
            .Include(user => user.Notices)
            .Include(user => user.UserEvents)
            .Include(user => user.OrganizerOrganization)
            .Include(user => user.FollowOrganization)
            .ToListAsync();

        return newUsers;
    }

    #endregion


}