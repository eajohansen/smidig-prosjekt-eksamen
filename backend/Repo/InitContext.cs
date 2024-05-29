using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using agile_dev.Models;

namespace agile_dev.Repo;

public class InitContext : DbContext{
    private readonly IConfiguration _configuration;

    public InitContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public InitContext(DbContextOptions<InitContext> options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }   
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        if (!optionsBuilder.IsConfigured) {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseMySQL(connectionString);
            
        }
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
    }

public DbSet<agile_dev.Models.Session> Session { get; set; } = default!;

public DbSet<agile_dev.Models.SessionDateTime> SessionDateTime { get; set; } = default!;
}