using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

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
    }    public virtual DbSet<Blog> Blog { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        if (!optionsBuilder.IsConfigured)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseMySQL(connectionString);
            
        }
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
    }
}