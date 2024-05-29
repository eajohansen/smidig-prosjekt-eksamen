using agile_dev.Models;
using agile_dev.Repo;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace agile_dev.Service;

public class UserService {
    private readonly InitContext _dbCon;

    public UserService(InitContext context) {
        _dbCon = context;
    }
    public async Task<IActionResult> AddUserToDatabase() {
        var newProfile = new Profile {
            FirstName = "John",
            LastName = "Doe",
            Birthdate = new DateTime(1990, 1, 1),
            ExtraInfo = "Extra info",
        };
        await _dbCon.Profile.AddAsync(newProfile);
        await _dbCon.SaveChangesAsync();
        var newUser = new User {
            Email = "test",
            Password = "yaya",
            ProfileId = newProfile.ProfileId,
        };
        await _dbCon.User.AddAsync(newUser);
        await _dbCon.SaveChangesAsync();

        return new OkObjectResult(newUser);
    }
}