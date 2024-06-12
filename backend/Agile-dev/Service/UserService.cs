using System.Globalization;
using agile_dev.Dto;
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

    public async Task<HandleReturn<List<UserFrontendDto>>> FetchAllUsers() {
        try {
            List<User> foundUsers = await _dbCon.User.ToListAsync();

            if (foundUsers.Count == 0) {
                return HandleReturn<List<UserFrontendDto>>.Failure("No users found.");
            }

            List<string> userIds = foundUsers.Select(user => user.Id).ToList();
            List<UserFrontendDto> fetchedUsers = ConvertUserToUserFrontendDtos(userIds);

            return HandleReturn<List<UserFrontendDto>>.Success(fetchedUsers);
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while fetching users.", exception);
        }
    }


    public async Task<HandleReturn<UserFrontendDto>> FetchUserById(string id) {
        try {
            User? user = await _dbCon.User.FindAsync(id);
            if (user == null) {
                return HandleReturn<UserFrontendDto>.Failure("Could not find user with this id");
            }
            List<User> users = [user];
            List<string> userIds = users.Select(user => user.Id).ToList();
            List<UserFrontendDto> fetchedUsers = ConvertUserToUserFrontendDtos(userIds);

            return HandleReturn<UserFrontendDto>.Success(fetchedUsers[0]);
        }
        catch (Exception exception) {
            Console.WriteLine(exception);
            throw new Exception("An error occurred while fetching user.", exception);
        }
    }

    public async Task<HandleReturn<UserFrontendDto>> FetchUserByEmail(string email) {
        try {
            User? user = await _userManager.FindByEmailAsync(email);
            if (user == null) {
                return HandleReturn<UserFrontendDto>.Failure("Could not find user with this email");
            }
            
            List<User> users = [user];
            List<string> userIds = users.Select(userObject => user.Id).ToList();
            List<UserFrontendDto> fetchedUsers = ConvertUserToUserFrontendDtos(userIds);

            return HandleReturn<UserFrontendDto>.Success(fetchedUsers[0]);
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while fetching user by email.", exception);
        }
    }
    
    public async Task<HandleReturn<bool>> CheckIfUserIsAdmin(string email) {
        try {
            User? user = await _userManager.FindByEmailAsync(email);
            if (user == null) {
                return HandleReturn<bool>.Failure("Could not find user with this email");
            }

            return HandleReturn<bool>.Success(await _userManager.IsInRoleAsync(user, "Admin"));
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
            return IdentityResult.Failed(new IdentityError { Description = "Could not find organization" });
        }

        if (IsUserOrganizeForOrganizer(user.Id, foundOrganization.OrganizationId).Result) {
            return IdentityResult.Failed(new IdentityError { Description = "User is already organizer for this organization" });
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

    public async Task<HandleReturn<bool>> AddUserAsFollower(string email, int organizationId) {
        try {
            User? user = await _userManager.FindByEmailAsync(email);

            if (user == null) {
                return HandleReturn<bool>.Failure("User not found");
            }
            
            Organization? foundOrganization = await _dbCon.Organization.FindAsync(organizationId);

            if (foundOrganization == null) {
                return HandleReturn<bool>.Failure("Organization not found");

            }

            if (IsUserFollowingOrganization(user.Id, foundOrganization.OrganizationId).Result) {
                return HandleReturn<bool>.Failure("User is already following the organization");

            }

            Follower newFollower = new() {
                UserId = user.Id,
                OrganizationId = organizationId
            };
            
            await _dbCon.Follower.AddAsync(newFollower);
            await _dbCon.SaveChangesAsync();
            return HandleReturn<bool>.Success();
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while adding user as follower.", exception);
        }
    }

    public async Task<HandleReturn<bool>> AddUserToEvent(string userEmail, int eventId) {
        try {
            User? user = await _userManager.FindByEmailAsync(userEmail);

            if (user == null) {
                return HandleReturn<bool>.Failure("User not found");
            }
            
            Event? foundEvent = await _dbCon.Event.FindAsync(eventId);

            if (foundEvent == null) {
                return HandleReturn<bool>.Failure("Event not found");
            }

            if (IsUserAttendingEvent(user.Id, foundEvent.EventId).Result) {
                return HandleReturn<bool>.Failure("User is already attending event");
            }

            UserEvent newUserEvent = new() {
                Id = user.Id,
                EventId = eventId,
                Used = false
            };

            await _dbCon.UserEvent.AddAsync(newUserEvent);
            await _dbCon.SaveChangesAsync();
            return HandleReturn<bool>.Success();
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while adding user to event.", exception);
        }
    }

    public async void AddNoticeToUser(User user) {
        try {
            Notice newNotice = new() {
                Expire = DateTime.Now.AddDays(30)
            };

            await _dbCon.Notice.AddAsync(newNotice);
            await _dbCon.SaveChangesAsync();
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
        
        user = AddRelationToUser([user]).Result[0];
        
        user.ExtraInfo = updatedUserInfo.Email;
        user.FirstName = updatedUserInfo.FirstName;
        user.LastName = updatedUserInfo.LastName;
        if (updatedUserInfo.Birthdate != null) {
            string yr = DateTime.Parse(updatedUserInfo.Birthdate.ToString()!).ToString(CultureInfo.InvariantCulture);
            user.Birthdate = DateTime.ParseExact(yr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
        }
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

    public async Task<HandleReturn<bool>> DeleteUser(string userEmail) {
        try {
            User? deleteuser = await _userManager.FindByEmailAsync(userEmail);

            if (deleteuser == null) {
                return HandleReturn<bool>.Failure("User not found");
            }

            _dbCon.User.Remove(deleteuser);
            await _dbCon.SaveChangesAsync();

            return HandleReturn<bool>.Success();
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while deleting user.", exception);
        }
    }

    #endregion

    #region MISCELLANEOUS

    private async Task<bool> IsUserFollowingOrganization(string userId, int organizationId) {

        Follower? isFollower = await _dbCon.Follower.Where(follower =>
            follower.UserId.Equals(userId) && follower.OrganizationId.Equals(organizationId)).FirstOrDefaultAsync();

        return isFollower != null;
    }

    private async Task<bool> IsUserAttendingEvent(string userId, int eventId) {

        UserEvent? isAttending = await _dbCon.UserEvent.Where(userEvent =>
            userEvent.Id.Equals(userId) && userEvent.EventId.Equals(eventId)).FirstOrDefaultAsync();

        return isAttending != null;
    }
    
    private async Task<bool> IsUserOrganizeForOrganizer(string userId, int organizationId) {

        Organizer? isOrganizer = await _dbCon.Organizer.Where(organizer =>
            organizer.UserId.Equals(userId) && organizer.OrganizationId.Equals(organizationId)).FirstOrDefaultAsync();

        return isOrganizer != null;
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

    private List<UserFrontendDto> ConvertUserToUserFrontendDtos(List<string> userIds) {
        List<UserFrontendDto> foundUsers = _dbCon.User
            .Where(user => userIds.Contains(user.Id))
            .Select(user => new UserFrontendDto {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Birthdate = user.Birthdate,
                ExtraInfo = user.ExtraInfo,
                FollowOrganization = user.FollowOrganization
                    .Select(fo => new Follower {
                        OrganizationId = fo.OrganizationId
                    }).ToList(),
                OrganizerOrganization = user.OrganizerOrganization
                    .Select(oo => new Organizer {
                        OrganizationId = oo.OrganizationId
                    }).ToList(),
                UserEvents = user.UserEvents
                    .Select(ue => new UserEvent {
                        EventId = ue.EventId
                    }).ToList(),
                Notices = user.Notices
                    .Select(n => new Notice {
                        Expire = n.Expire
                    }).ToList(),
                Allergies = user.Allergies
                    .Select(a => new Allergy {
                        Name = a.Name,
                        Description = a.Description
                    }).ToList()
            }).ToList();
        return foundUsers;
    }
    
    #endregion
}