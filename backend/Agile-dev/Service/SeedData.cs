using agile_dev.Models;
using Microsoft.AspNetCore.Identity;

namespace agile_dev
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

            // Create the Admin user who will maintain the web app
            // Getting the username and password from appsettings.json
            var adminSettings = configuration.GetSection("AdminUser");
            var powerUser = new User
            {
                UserName = adminSettings["Username"],
                Email = adminSettings["Email"],
            };

            string userPassword = adminSettings["Password"];
            var user = await userManager.FindByEmailAsync(adminSettings["Email"]);

            if (user == null)
            {
                IdentityResult createPowerUser = await userManager.CreateAsync(powerUser, userPassword);
                if (createPowerUser.Succeeded)
                {
                    // Assign the new user the Admin role
                    await userManager.AddToRoleAsync(powerUser, "Admin");
                }
            }
        }
    }
}