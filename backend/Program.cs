using agile_dev.Repo;
using agile_dev.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace agile_dev;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        // CHANGE THIS TO BE MORE SPECIFIC FOR CORS, WILL FAIL WITH AUTH AS IS
        builder.Services.AddCors(
            options => {
                options.AddPolicy("AllowAnyOrigin",
                policies => policies
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                );
            }
        );
        builder.Services.AddControllers();
        builder.Services.AddScoped<UserService>();

        builder.Services.AddDbContext<InitContext>(options =>
            options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")));
        // Add services to the container.
        builder.Services.AddAuthorization();
        builder.Services.AddIdentityApiEndpoints<IdentityUser>()
            .AddEntityFrameworkStores<InitContext>();

        var app = builder.Build();
        app.UseCors("AllowAnyOrigin");
        
        //app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();

        using (var scope = app.Services.CreateScope()) {
            IServiceProvider services = scope.ServiceProvider;
            InitContext dbContext = services.GetRequiredService<InitContext>();
            dbContext.Database.Migrate();
        }

        app.UseEndpoints(endpoints => {
            endpoints.MapControllers();
        });
        app.Run();
    }
}