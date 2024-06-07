using System.Text;
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
        builder.Services.AddControllers();
        builder.Services.AddScoped<UserService>();
        builder.Services.AddScoped<EventService>();
        builder.Services.AddScoped<OrganizationService>();
        builder.Services.AddDbContext<InitContext>(options =>
            options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddIdentityApiEndpoints<IdentityUser>().AddEntityFrameworkStores<InitContext>();

        builder.Services.AddAuthentication();
   
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();

        var app = builder.Build();
        app.MapIdentityApi<IdentityUser>();

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
        }

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
        app.Run();
    }
}