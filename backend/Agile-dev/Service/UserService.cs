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

    public async Task<IActionResult> AddUserToDatabase(User user) {
        await _dbCon.User.AddAsync(user);
        await _dbCon.SaveChangesAsync();

        return new OkObjectResult(user);
    }

    public async Task<IActionResult> AddProfileToDatabase(User user, List<Allergy> allergies) {
        await _dbCon.User.AddAsync(user);
        await _dbCon.SaveChangesAsync();
    
        foreach (Allergy allergy in allergies) {
            allergy.UserId = user.UserId;
            allergy.User = user;
            await _dbCon.Allergy.AddAsync(allergy);
            await _dbCon.SaveChangesAsync();
        }
    
        return new OkObjectResult(user);
    }

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
}