using System.Text;
using System.Text.Json.Serialization;
using agile_dev.Models;
using agile_dev.Repo;
using agile_dev.Service;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace agile_dev;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddHttpLogging(logging => {
            logging.LoggingFields = HttpLoggingFields.Request;
            logging.RequestHeaders.Add("Referer");
            logging.ResponseHeaders.Add("MyResponseHeader");
        });
        builder.Services.AddCors(
            options => {
                options.AddPolicy("_frontendCorsPolicy",
                policies => policies
                    .WithOrigins("http://localhost:3000")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                );
            }
        );
        builder.Services.AddControllers().AddJsonOptions(options => {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });
        builder.Services.AddScoped<UserService>();
        builder.Services.AddScoped<EventService>();
        builder.Services.AddScoped<OrganizationService>();
        builder.Services.AddDbContextPool<InitContext>(options =>
            options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection"), mysqlOptions => {
                mysqlOptions.EnableRetryOnFailure();
            }));
        builder.Services.AddIdentityApiEndpoints<User>(options =>
                   {
                       // Password settings.
                       options.Password.RequireDigit = true;
                       options.Password.RequireLowercase = true;
                       options.Password.RequireNonAlphanumeric = true;
                       options.Password.RequireUppercase = true;
                       options.Password.RequiredLength = 8;
                       options.Password.RequiredUniqueChars = 1;

                       // Lockout settings.
                       options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                       options.Lockout.MaxFailedAccessAttempts = 5;
                       options.Lockout.AllowedForNewUsers = true;

                       // User settings.
                       options.User.RequireUniqueEmail = true;
                   })
                   .AddRoles<IdentityRole>()
                   .AddEntityFrameworkStores<InitContext>();
        
        builder.Services.AddAuthentication();
   
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();

        var app = builder.Build();
        app.MapIdentityApi<User>();

        //app.UseHttpsRedirection();
        app.UseRouting();

        app.UseCors("_frontendCorsPolicy");
        app.UseHttpLogging();
        app.UseAuthentication();
        app.UseAuthorization();

        using (var scope = app.Services.CreateScope()) {
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<InitContext>();
            context.Database.Migrate();
            
            // Call the SeedData to create roles and the first admin user
            var configuration = services.GetRequiredService<IConfiguration>();
            SeedData.Initialize(services, configuration).Wait();
        }

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
        app.Run();
    }
}