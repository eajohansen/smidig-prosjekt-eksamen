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
            return foundUsers;
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while fetching users.", exception);
        }
    }

    public async Task<User?> FetchUserById(int id) {
        try {
            User? user = await _dbCon.User.FindAsync(id);
            return user;
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while fetching user.", exception);
        }
    }

    #endregion

    #region POST

    public async Task<bool> AddUserToDatabase(User user) {
        try {
            await _dbCon.User.AddAsync(user);
            await _dbCon.SaveChangesAsync();
            return true;
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while adding user to database.", exception);
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

    public async Task<bool> MakeUserAdmin(User adminUser, User user) {
        try {
            bool isUserAdmin = IsUserAdmin(adminUser).Result;
            if (!isUserAdmin) {
                return false;
            }

            User? databaseUser = FetchUserById(user.UserId).Result;
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

    #endregion
    
    #region GENERAL

    public async Task<bool> IsUserAdmin(User user) {
        User? databaseAdminUser = FetchUserById(user.UserId).Result;
        if (databaseAdminUser == null) {
            return false;
        }

        return databaseAdminUser.Admin;
    }
    
    #endregion
    
    
}