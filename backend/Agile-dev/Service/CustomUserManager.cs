using Microsoft.AspNetCore.Identity;
using agile_dev.Models;
using Microsoft.Extensions.Options;

namespace agile_dev.Service

/*
 * Custom UserManager only intercepts the register-event to make the first user that registers
 * admin in the application.
 */
{
    public class CustomUserManager : UserManager<User>
    {
        private readonly IServiceProvider _serviceProvider;

        public CustomUserManager(IServiceProvider serviceProvider, IUserStore<User> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<User> passwordHasher, IEnumerable<IUserValidator<User>> userValidators, IEnumerable<IPasswordValidator<User>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<User>> logger)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _serviceProvider = serviceProvider;
        }

        public override async Task<IdentityResult> CreateAsync(User user, string password)
        {
            var result = await base.CreateAsync(user, password);
            if (result.Succeeded)
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
                    if (Users.Count() == 1)
                    {
                        await AddToRoleAsync(user, "Admin");
                    }
                }
            }
            return result;
        }
    }
}