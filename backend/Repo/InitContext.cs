using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using agile_dev.Models;

namespace agile_dev.Repo;

public class InitContext : DbContext{
    private readonly IConfiguration _configuration;
    
    public DbSet<User> User { get; set; }
    public DbSet<Profile> Profile { get; set; }
    public DbSet<Allergy> Allergy { get; set; }
    
    public InitContext(IConfiguration configuration) {
        _configuration = configuration;
    }
    public InitContext() {
        _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        if (!optionsBuilder.IsConfigured) {
            optionsBuilder.UseMySQL("Server=database,9999;Database=agile-project;User=root;Password=agileavengers;");
        }
        
    }

  

}