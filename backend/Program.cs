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
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                );
            }
        );
        builder.Services.AddControllers();
        builder.Services.AddScoped<UserService>();
        builder.Services.AddDbContext<InitContext>(options =>
            options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddIdentityApiEndpoints<IdentityUser>().AddEntityFrameworkStores<InitContext>();
        
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Issuer"],
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });
        builder.Services.AddAuthorization();
        var app = builder.Build();

        app.UseCors("_frontendCorsPolicy");

        app.MapIdentityApi<IdentityUser>();
        
        //app.UseHttpsRedirection();
        app.UseRouting();
        using (var scope = app.Services.CreateScope()) {
            IServiceProvider services = scope.ServiceProvider;
            InitContext dbContext = services.GetRequiredService<InitContext>();
            dbContext.Database.Migrate();
        }
        app.UseHttpLogging();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => {
            endpoints.MapControllers();
        });
        app.Run();
    }
}