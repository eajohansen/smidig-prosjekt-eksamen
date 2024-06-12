using Microsoft.AspNetCore.Identity;


namespace agile_dev.Service
{
    public class FirstUserAdminValidator : IUserValidator<IdentityUser>
    {
        private readonly IServiceProvider _serviceProvider;

        public FirstUserAdminValidator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<IdentityResult> ValidateAsync(UserManager<IdentityUser> manager, IdentityUser user)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                // Ensure the Admin role exists
                if (!await roleManager.RoleExistsAsync("Admin"))
                {
                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                }

                // Check if this is the first user
                if (manager.Users.Count() == 1)
                {
                    await manager.AddToRoleAsync(user, "Admin");
                }
            }
            return IdentityResult.Success;
        }
    }
}