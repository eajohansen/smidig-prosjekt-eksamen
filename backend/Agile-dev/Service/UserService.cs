using System.Collections;
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
            ICollection<User> newUsers = await AddRelationToUser(foundUsers.ToList());
            
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
                foundUser = await AddRelationToUser(foundUser);
                return foundUser[0];
            } else {
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

    public async Task<bool> AddUserToDatabase(User user) {
        try {
            
            if (!_dbCon.User.Any()) {
                user.Admin = true;
            }

            ICollection<Allergy> allergies = user.Allergies.ToList();

            user.Allergies.Clear();
            
            await _dbCon.User.AddAsync(user);
            await _dbCon.SaveChangesAsync();

            User? databaseUser;
            try {
                databaseUser = await FetchUserByEmail(user.Email);
                if (databaseUser == null) {
                    return false;
                }
            }
            catch (Exception exception) {
                throw new Exception("Cant find user by email", exception);
            }
            
            if (allergies.Count != 0) {
                foreach (Allergy allergy in allergies) {
                    allergy.UserId = databaseUser.UserId;
                    await _dbCon.Allergy.AddAsync(allergy);
                }
            }
            
            await _dbCon.SaveChangesAsync();
            return true;
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while adding user to database.", exception);
        }
    }

    public async Task<bool> AddUserAsOrganizer(int loggedInUserId, User user, int organizationId) {
        try {
            User? foundUser = await _dbCon.User.FindAsync(loggedInUserId);
            Organization? foundOrganization = await _dbCon.Organization.FindAsync(organizationId);
            
            if (foundUser != null && foundOrganization != null && !IsUserOrganizerForOrganization(user, foundOrganization).Result && (!IsUserOrganizerForOrganization(foundUser, foundOrganization).Result || !IsUserAdmin(foundUser).Result)) {
                return false;
            }
            
            Organizer newOrganizer = new Organizer(user.UserId, organizationId);
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
            
            if (foundOrganization != null && !IsUserFollowingOrganization(user, foundOrganization).Result) {
                return false;
            }

            Follower newFollower = new Follower(user.UserId, organizationId);
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

            UserEvent newUserEvent = new UserEvent(user.UserId, eventId) {
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

            Notice newNotice = new Notice(user.UserId) {
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
            _dbCon.User.Update(user);
            await _dbCon.SaveChangesAsync();
            return true;
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while updating user.", exception);
        }
    }

    public async Task<bool> UpdateUserFirstName(User user) {
        try {
            User? databaseUser = FetchUserById(user.UserId).Result;
            if (databaseUser == null) {
                return false;
            }

            databaseUser.FirstName = user.FirstName;
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

    public async Task<bool> UpdateUserAllergiesDb(User user) {
        try {
            User? dbUser = await FetchUserById(user.UserId);

            if (dbUser?.Allergies != null && dbUser?.Allergies.Count != 0) {
                foreach (Allergy allergy in dbUser.Allergies) { _dbCon.Allergy.Remove(allergy); }

                dbUser.Allergies.Clear();

                if (user.Allergies.Count != 0) {
                    foreach (Allergy allergy in user.Allergies) {
                        allergy.UserId = user.UserId;
                        await _dbCon.Allergy.AddAsync(allergy);
                    }
                }
            }

            await _dbCon.SaveChangesAsync();
            return true;
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while updating user allergies.", exception);
        }
    }

    #endregion

    #region DELETE

    #endregion
    
    #region MISCELLANEOUS

    public async Task<bool> IsUserAdmin(User user) {
        User? databaseAdminUser = await FetchUserById(user.UserId);
        return databaseAdminUser is { Admin: true };
    }

    public async Task<bool> IsUserOrganizerForOrganization(User user, Organization organization) {
        User? databaseUser = await FetchUserById(user.UserId);
        return databaseUser != null && databaseUser.OrganizerOrganization.Any(organizations => organizations.OrganizationId.Equals(organization.OrganizationId));
    }
    
    private async Task<bool> IsUserFollowingOrganization(User user, Organization organization) {
        User? databaseUser = await FetchUserById(user.UserId);
        return databaseUser != null && databaseUser.FollowOrganization.Any(organizations => organizations.OrganizationId.Equals(organization.OrganizationId));
    }
    
    private async Task<bool> IsUserAttendingEvent(User user, Event eEvent) {
        User? databaseUser = await FetchUserById(user.UserId);
        return databaseUser != null && databaseUser.UserEvents.Any(userEvent => userEvent.EventId.Equals(eEvent.EventId));
    }

    private async Task<List<User>> AddRelationToUser(List<User> users) {
        List<int> userIds = users.Select(u => u.UserId).ToList();
    
        List<User> newUsers = await _dbCon.User
            .Where(u => userIds.Contains(u.UserId))
            .Include(u => u.Allergies)
            .Include(u => u.Notices)
            .Include(u => u.UserEvents)
            .Include(u => u.OrganizerOrganization)
            .Include(u => u.FollowOrganization)
            .ToListAsync();

        return newUsers;
    }

    
    #endregion
}