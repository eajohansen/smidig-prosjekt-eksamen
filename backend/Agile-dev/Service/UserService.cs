using agile_dev.Models;
using agile_dev.Repo;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace agile_dev.Service;

public class UserService {
    private readonly InitContext _dbCon;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserService(InitContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager) {
        _dbCon = context;
        _userManager = userManager;
        _roleManager = roleManager;
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
    

    
    public async Task<object> FetchUserById(string id) {
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
    

    public async Task<User?> FetchUserByEmail(string email) {
        try {
            User? user = await _userManager.FindByEmailAsync(email);
            return user;
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while fetching user by email.", exception);
        }
    }

    #endregion

    
    #region POST

    public async Task<IdentityResult> AddUserAsOrganizer(string email)
    {
        const int organizationId = 1;
        
        User? user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return IdentityResult.Failed(new IdentityError { Description = $"User with email {email} was not found." });
        }

        Organization? foundOrganization = await _dbCon.Organization.FindAsync(organizationId);

        if (foundOrganization == null) {
            return IdentityResult.Failed(new IdentityError { Description = $"Could not find organization" });
        }
        
        IdentityResult result = await _userManager.AddToRoleAsync(user, "Organizer");

        Organizer newOrganizer = new () {
            Id = user.Id,
            OrganizationId = organizationId
        };
        await _dbCon.Organizer.AddAsync(newOrganizer);
        await _dbCon.SaveChangesAsync();
        return result;

    }
    

    public async Task<bool> AddUserAsFollower(string userEmail, int organizationId) {
        try {
            Organization? foundOrganization = await _dbCon.Organization.FindAsync(organizationId);
            User? user = await _userManager.FindByEmailAsync(userEmail);
            
            if (foundOrganization == null || !IsUserFollowingOrganization(userEmail, foundOrganization).Result) {
                return false;
            }
            
            Follower newFollower = new () {
                Id = user.Id,
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

    public async Task<bool> AddUserToEvent(string userEmail, int eventId) {
        try {
            Event? foundEvent = await _dbCon.Event.FindAsync(eventId);
            User? user = await _userManager.FindByEmailAsync(userEmail);

            if (foundEvent != null && user != null && !IsUserAttendingEvent(user, foundEvent).Result) {
                return false;
            }

            UserEvent newUserEvent = new () {
                Id = user.Id,
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
                Id = user.Id,
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

        public async Task<IdentityResult> UpdateUserAsync(string userEmail, User updatedUserInfo)
        {
            User? user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"User with email {userEmail} not found." });
            }

            user.FirstName = updatedUserInfo.FirstName;
            user.LastName = updatedUserInfo.LastName;
            user.Birthdate = updatedUserInfo.Birthdate;
            user.ExtraInfo = updatedUserInfo.ExtraInfo;
            
            // Update allergies if provided
            if (updatedUserInfo.Allergies != null)
            {
                // Clear existing allergies
                user.Allergies.Clear();

                // Add updated allergies
                foreach (Allergy allergy in updatedUserInfo.Allergies)
                {
                    user.Allergies.Add(new Allergy
                    {
                        AllergyId = allergy.AllergyId,
                        Name = allergy.Name,
                        Description = allergy.Description,
                        Id = user.Id, // Ensure the correct user id is set
                    });
                }
            }

            // Update any other fields as necessary

            var result = await _userManager.UpdateAsync(user);
            return result;
        }
    

    public async Task<IdentityResult> MakeUserAdmin(string email) {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return IdentityResult.Failed(new IdentityError { Description = $"User with email {email} does not exist." });
        }

        var result = await _userManager.AddToRoleAsync(user, "Admin");
        return result;
    }

    #endregion

    #region DELETE
    public async Task<bool> DeleteUser(string userEmail) {
        try
        {
            User? deleteuser = await _userManager.FindByEmailAsync(userEmail);
            
            if (deleteuser == null) {
                return false;
            }
            
            _dbCon.User.Remove(deleteuser);
            await _dbCon.SaveChangesAsync();
            
            return true;
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while deleting user.", exception);
        }
    }

    #endregion

    #region MISCELLANEOUS

    public async Task<bool> IsUserAdmin(User user)
    {
        var identityUser = await _userManager.FindByIdAsync(user.Id);
        if (identityUser == null)
        {
            return false;
        }

        return await _userManager.IsInRoleAsync(identityUser, "Admin");
    }

    private async Task<bool> IsUserFollowingOrganization(string userEmail, Organization organization) {
        object databaseUser = await FetchUserByEmail(userEmail);
        if (databaseUser is not User realUser) {
            return false;
        }
        
        // Loops through all the organizations the user is following, and returns true if we find the id of the organization we are looking for
        return realUser.FollowOrganization != null && realUser.FollowOrganization.Any(organizations =>
            organizations.OrganizationId.Equals(organization.OrganizationId));
    }

    private async Task<bool> IsUserAttendingEvent(User user, Event eEvent) {
        object databaseUser = await FetchUserById(user.Id);
        if (databaseUser is not User realUser) {
            return false;
        }
        
        // Loops through all the events the user is attending, and returns true if we find the id of the event we are looking for
        return realUser.UserEvents != null && realUser.UserEvents.Any(userEvent => userEvent.EventId.Equals(eEvent.EventId));
    }

    private async Task<List<User>> AddRelationToUser(List<User> users) {
        List<string> userIds = users.Select(user => user.Id).ToList();

        List<User> newUsers = await _dbCon.User
            .Where(user => userIds.Contains(user.Id))
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