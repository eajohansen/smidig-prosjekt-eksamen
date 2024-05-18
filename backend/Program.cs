using agile_dev.Repo;
using Microsoft.EntityFrameworkCore;

namespace agile_dev;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<InitContext>(options =>
            options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")));
        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        //app.UseHttpsRedirection();

        app.UseAuthorization();
        
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<InitContext>();    
            context.Database.Migrate();
        }

        var summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        app.MapPost("/test", (HttpContext httpContext) => {
            using (var db = new InitContext(builder.Configuration))
            {
                // Create and save a new Blog
                Console.Write("Enter a name for a new Blog: ");
                var name = Console.ReadLine();

                var blog = new Blog { Name = "name", Url = "test"};
                db.Blog.Add(blog);
                db.SaveChanges();
                return blog;

            }
        });

    app.MapGet("/weatherforecast", (HttpContext httpContext) =>
            {
                var forecast = Enumerable.Range(1, 5).Select(index =>
                        new WeatherForecast
                        {
                            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                            TemperatureC = Random.Shared.Next(-20, 55),
                            Summary = summaries[Random.Shared.Next(summaries.Length)]
                        })
                    .ToArray();
                return forecast;
            })
            .WithName("GetWeatherForecast")
            .WithOpenApi();
        app.Run();
    }
}