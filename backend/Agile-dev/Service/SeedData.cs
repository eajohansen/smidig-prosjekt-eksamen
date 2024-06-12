using agile_dev.Models;
using Microsoft.AspNetCore.Identity;

namespace Agile_dev.Service
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Get the roles from appsettings.json
            var roles = configuration.GetSection("Roles").Get<string[]>();
            IdentityResult roleResult;

            foreach (var roleName in roles)
            {
                bool roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    // Create the roles and seed them to the database
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
    }
}