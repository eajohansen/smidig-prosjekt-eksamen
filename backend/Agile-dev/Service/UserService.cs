using System.Security.Claims;
using agile_dev.Dto;
using agile_dev.Models;
using agile_dev.Repo;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

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

    public async Task<List<UserFrontendDto>> FetchAllUsers() {
        try {
            List<User> foundUsers = await _dbCon.User.ToListAsync();
            foundUsers = await AddRelationToUser(foundUsers);
            ICollection<UserFrontendDto> userFrontendDtos = foundUsers.Select(user => new UserFrontendDto() {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Birthdate = user.Birthdate,
                    ExtraInfo = user.ExtraInfo,
                    FollowOrganization = user.FollowOrganization,
                    OrganizerOrganization = user.OrganizerOrganization,
                    UserEvents = user.UserEvents,
                    Notices = user.Notices,
                    Allergies = user.Allergies
                })
                .ToList();

            return userFrontendDtos.ToList();

            //return foundUsers.Select(user => AddRelationToUser(user).Result).ToList();
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while fetching users.", exception);
        }
    }


    public async Task<object> FetchUserById(string id) {
        try {
            User? user = await _dbCon.User.FindAsync(id);
            if (user == null) return "Could not find user with this id";
            List<User> users = [user];
            user = AddRelationToUser(users).Result[0];
            return new UserFrontendDto() {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Birthdate = user.Birthdate,
                ExtraInfo = user.ExtraInfo,
                FollowOrganization = user.FollowOrganization,
                OrganizerOrganization = user.OrganizerOrganization,
                UserEvents = user.UserEvents,
                Notices = user.Notices,
                Allergies = user.Allergies
            };

        }
        catch (Exception exception) {
            Console.WriteLine(exception);
            throw new Exception("An error occurred while fetching user.", exception);
        }
    }

    public async Task<UserFrontendDto?> FetchUserByEmail(string email) {
        try {
            User? user = await _userManager.FindByEmailAsync(email);
            List<User> users = [user];
            user = AddRelationToUser(users).Result[0];
            return new UserFrontendDto() {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Birthdate = user.Birthdate,
                ExtraInfo = user.ExtraInfo,
                FollowOrganization = user.FollowOrganization,
                OrganizerOrganization = user.OrganizerOrganization,
                UserEvents = user.UserEvents,
                Notices = user.Notices,
                Allergies = user.Allergies
            };
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while fetching user by email.", exception);
        }
    }

    #endregion

    #region POST

    public async Task<IdentityResult> AddUserAsOrganizer(int organizationId, string email) {

        User? user = await _userManager.FindByEmailAsync(email);
        if (user == null) {
            return IdentityResult.Failed(new IdentityError { Description = $"User with email {email} was not found." });
        }

        Organization? foundOrganization = await _dbCon.Organization.FindAsync(organizationId);

        if (foundOrganization == null) {
            return IdentityResult.Failed(new IdentityError { Description = $"Could not find organization" });
        }

        IdentityResult result = await _userManager.AddToRoleAsync(user, "Organizer");

        Organizer newOrganizer = new() {
            OrganizationId = organizationId,
            UserId = user.Id
        };
        await _dbCon.Organizer.AddAsync(newOrganizer);
        await _dbCon.SaveChangesAsync();
        return result;
    }

    public async Task<IdentityResult> AddUserAsFollower(int organizationId) {
        try {
            Organization? foundOrganization = await _dbCon.Organization.FindAsync(organizationId);
            User? user = await _userManager.FindByEmailAsync(ClaimTypes.Email);

            if (user?.Email == null) {
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });
            }

            if (foundOrganization == null || !IsUserFollowingOrganization(user.Email, foundOrganization).Result) {
                return IdentityResult.Failed(new IdentityError { Description = "Organization not found" });
            }

            Follower newFollower = new() {
                Id = user.Id,
            };
            await _dbCon.Follower.AddAsync(newFollower);
            await _dbCon.SaveChangesAsync();
            return IdentityResult.Success;
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while adding user as follower.", exception);
        }
    }

    public async Task<IdentityResult> AddUserToEvent(string userEmail, int eventId) {
        try {
            Event? foundEvent = await _dbCon.Event.FindAsync(eventId);

            if (foundEvent == null) {
                return IdentityResult.Failed(new IdentityError { Description = "Event not found" });
            }
            
            User? user = await _userManager.FindByEmailAsync(userEmail);

            if (user == null) {
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });
            }

            // if (!IsUserAttendingEvent(user, foundEvent).Result) {
            //     return IdentityResult.Failed(new IdentityError { Description = "User is already attending event" });
            // }

            Console.WriteLine("1");
            UserEvent newUserEvent = new() {
                Id = user.Id,
                EventId = eventId,
                Used = false
            };
            Console.WriteLine("2");

            await _dbCon.UserEvent.AddAsync(newUserEvent);
            Console.WriteLine("3");
            await _dbCon.SaveChangesAsync();
            Console.WriteLine("4");
            return IdentityResult.Success;
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while adding user to event.", exception);
        }
    }

    public async Task<bool> AddNoticeToUser(User user) {
        try {
            Notice newNotice = new() {
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

    public async Task<IdentityResult> UpdateUserAsync(User updatedUserInfo) {
        
        // Need to add check for received email matching logged-in user mail
        
        User? user = await _userManager.FindByEmailAsync(updatedUserInfo.Email!);

        if (user == null) {
            return IdentityResult.Failed(new IdentityError
                { Description = $"User with email {updatedUserInfo.Email!} not found." });
        }
        
        user = AddRelationToUser([user]).Result[0];
        
        user.ExtraInfo = updatedUserInfo.Email;
        user.FirstName = updatedUserInfo.FirstName;
        user.LastName = updatedUserInfo.LastName;
        user.Birthdate = updatedUserInfo.Birthdate;
        user.ExtraInfo = updatedUserInfo.ExtraInfo;

        // Update allergies if provided

        // Clear existing allergies
        User? databaseUser = await _dbCon.User.FindAsync(user.Id);
        if (databaseUser == null) {
            return IdentityResult.Failed(new IdentityError
                { Description = $"User with email {updatedUserInfo.Email!} not found." });
        }

        if (user.Allergies != null) {
            foreach (Allergy allergy in user.Allergies) {
                _dbCon.Allergy.Remove(allergy);
            }

            await _dbCon.SaveChangesAsync();


            user.Allergies.Clear();
        }
        else {
            user.Allergies = new List<Allergy>();
        }

        if (updatedUserInfo.Allergies != null) {
            foreach (Allergy allergy in updatedUserInfo.Allergies) {
                user.Allergies.Add(new Allergy {
                    Name = allergy.Name,
                    Description = allergy.Description
                });
            }
        }

        IdentityResult result = await _userManager.UpdateAsync(user);
        return result;
    }


    public async Task<IdentityResult> MakeUserAdmin(string email) {
        User? user = await _userManager.FindByEmailAsync(email);
        if (user == null) {
            return IdentityResult.Failed(new IdentityError
                { Description = $"User with email {email} does not exist." });
        }

        IdentityResult result = await _userManager.AddToRoleAsync(user, "Admin");
        return result;
    }

    #endregion

    #region DELETE

    public async Task<bool> DeleteUser(string userEmail) {
        try {
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

    public async Task<bool> IsUserAdmin(User user) {
        var identityUser = await _userManager.FindByIdAsync(user.Id);
        if (identityUser == null) {
            return false;
        }

        return await _userManager.IsInRoleAsync(identityUser, "Admin");
    }

    private async Task<bool> IsUserFollowingOrganization(string userEmail, Organization organization) {
        object databaseUser = await FetchUserByEmail(userEmail);
        if (databaseUser is not User realUser) {
            return false;
        }

        return true;

        // Loops through all the organizations the user is following, and returns true if we find the id of the organization we are looking for
        // We set return true temporary, for testing purposes
        // return realUser.FollowOrganization != null && realUser.FollowOrganization.Any(organizations =>
        //     organizations.OrganizationId.Equals(organization.OrganizationId));
    }

    private async Task<bool> IsUserAttendingEvent(User user, Event eEvent) {
        object databaseUser = await FetchUserById(user.Id);
        if (databaseUser is not User realUser) {
            return false;
        }

        // Loops through all the events the user is attending, and returns true if we find the id of the event we are looking for
        // We set return true temporary, for testing purposes
        // return realUser.UserEvents != null && realUser.UserEvents.Any(userEvent => userEvent.EventId.Equals(eEvent.EventId));
        return true;
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
    
    private async Task<UserFrontendDto?> AddRelationToUsefr(User user) {
        var newUser = await _dbCon.User
            .Where(u => u.Id == user.Id)
            .Include(userFrontendDto => user.FollowOrganization)
            .Include(userFrontendDto => user.OrganizerOrganization)
            .Include(userFrontendDto => user.UserEvents)
            .Include(userFrontendDto => user.Notices)
            .Include(userFrontendDto => user.Allergies)
            .Select(u => new UserFrontendDto {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Birthdate = user.Birthdate,
                ExtraInfo = user.ExtraInfo,
                FollowOrganization = user.FollowOrganization,
                OrganizerOrganization = user.OrganizerOrganization,
                UserEvents = user.UserEvents,
                Notices = user.Notices,
                Allergies = user.Allergies
            })
            
            .SingleOrDefaultAsync();
        Console.WriteLine("hheheheheeheheheh");
        return newUser;
    }


    #endregion
}