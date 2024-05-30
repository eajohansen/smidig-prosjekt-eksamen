using agile_dev.Repo;
using agile_dev.Service;
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

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();
        app.UseCors("AllowAnyOrigin");

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        //app.UseHttpsRedirection();
        app.UseRouting();
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