using agile_dev.Models;
using agile_dev.Repo;
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

    /*public async Task<ICollection<User>> FetchAllUsers() {
        try {
            ICollection<User> foundUsers = await _dbCon.User.ToListAsync();
            ICollection<User> newUsers = AddRelationToUser(foundUsers.ToList()).Result;

            return newUsers;
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while fetching users.", exception);
        }
    }
    */

    /*
    public async Task<object> FetchUserById(int id) {
        try {
            User? user = await _dbCon.User.FindAsync(id);
            if (user != null) {
                List<User> foundUser = [user];
                foundUser = AddRelationToUser(foundUser).Result;
                return foundUser[0];
            }
            else {
                return "Could not find user with this id";
            }
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while fetching user.", exception);
        }
    }
    */

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

    
  //  #region POST

    public async Task<object> AddUserToDatabase(User user) {
        try {
            if (!_dbCon.User.AnyAsync().Result) {
                user.Admin = true;
            }

            User? userExists = await FetchUserByEmail(user.Email);

            if (userExists != null) {
                return "User already exists.";
            }
            
            List<Allergy> allergies = new List<Allergy>();
            if(user.Allergies is { Count: > 0 }) {
               allergies = user.Allergies.ToList();
            }
            User newUser = user;
            newUser.Allergies = new List<Allergy>(allergies);
            
            await _dbCon.User.AddAsync(newUser); 
            await _dbCon.SaveChangesAsync();
            return newUser;
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while adding user to database.", exception);
        }
    }
/*
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

            Organizer newOrganizer = new () {
                UserId = user.UserId,
                OrganizationId = organizationId
            };
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

            Follower newFollower = new () {
                UserId = user.UserId,
                OrganizationId = organizationId
            };
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

            UserEvent newUserEvent = new () {
                UserId = user.UserId,
                EventId = eventId,
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
            Notice newNotice = new () {
                UserId = user.UserId,
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

    public async Task<object> UpdateUser(User user) {
        try {
            object databaseUser = await FetchUserById(user.UserId);
            if (databaseUser is not User realUser) {
                return "Could not find the user to update them";
            }
            // Check if databaseUser and user have the same authorization level
            if (user.Admin != realUser.Admin) {
                return "User has a different authorization privilege";
            }
            
            // Handle allergies

            // databaseUser.UserEvents = new List<UserEvent>();
            // databaseUser.Allergies = new List<Allergy>();
            // databaseUser.FollowOrganization = new List<Follower>();
            // databaseUser.OrganizerOrganization = new List<Organizer>();
            // databaseUser.Notices = new List<Notice>();
            
            if (realUser.Allergies != null && realUser.Allergies.Count != 0) {
                foreach (Allergy allergy in realUser.Allergies) {
                    _dbCon.Allergy.Remove(allergy);
                }
            }
            
            if (user.Allergies != null && user.Allergies.Count != 0) {
                foreach (Allergy allergy in user.Allergies) {
                    allergy.UserId = user.UserId;
                    await _dbCon.Allergy.AddAsync(allergy);
                }
            }
            
            realUser.Email = user.Email;
            realUser.Birthdate = user.Birthdate;
            realUser.ExtraInfo = user.ExtraInfo;
            realUser.FirstName = user.FirstName;
            realUser.LastName = user.LastName;
            
                _dbCon.User.Update(realUser);
                
            await _dbCon.SaveChangesAsync();
            
            return realUser;
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while updating user.", exception);
        }
    }

    public async Task<object> MakeUserAdmin(User adminUser, int id) {
        try {
            bool isUserAdmin = IsUserAdmin(adminUser).Result;
            if (!isUserAdmin) {
                return "User does not have admin privilege";
            }

            object databaseUser = FetchUserById(id).Result;
            if (databaseUser is not User realUser) {
                return "Could not fetch user by id";
            }

            realUser.Admin = true;

            // databaseUser.Role = "admin";
            _dbCon.User.Update(realUser);
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
        object databaseAdminUser = await FetchUserById(user.UserId);
        return (User)databaseAdminUser is { Admin: true };
    }

    public async Task<bool> IsUserOrganizerForOrganization(User user, Organization organization) {
        object databaseUser = await FetchUserById(user.UserId);
        if (databaseUser is not User realUser) {
            return false;
        }
        
        // Loops through all the organizations the user is an organizer for, and returns true if we find the id of the organization we are looking for
        return realUser.OrganizerOrganization != null && realUser.OrganizerOrganization.Any(organizations =>
            organizations.OrganizationId.Equals(organization.OrganizationId));
    }

    private async Task<bool> IsUserFollowingOrganization(User user, Organization organization) {
        object databaseUser = await FetchUserById(user.UserId);
        if (databaseUser is not User realUser) {
            return false;
        }
        
        // Loops through all the organizations the user is following, and returns true if we find the id of the organization we are looking for
        return realUser.FollowOrganization != null && realUser.FollowOrganization.Any(organizations =>
            organizations.OrganizationId.Equals(organization.OrganizationId));
    }

    private async Task<bool> IsUserAttendingEvent(User user, Event eEvent) {
        object databaseUser = await FetchUserById(user.UserId);
        if (databaseUser is not User realUser) {
            return false;
        }
        
        // Loops through all the events the user is attending, and returns true if we find the id of the event we are looking for
        return realUser.UserEvents != null && realUser.UserEvents.Any(userEvent => userEvent.EventId.Equals(eEvent.EventId));
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
    */


}