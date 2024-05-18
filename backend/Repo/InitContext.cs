using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace agile_dev.Repo;

public class InitContext : DbContext{
    public InitContext() {}
    public InitContext(DbContextOptions<InitContext> options) : base(options) {}
    public virtual DbSet<Blog> Blog { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        /* For denne eksamen vil vi inkludere dette, men generelt sett skal det være plassert i en annen fil og refere til den derfra */
        optionsBuilder.UseMySQL("Server=host.docker.internal;port=3306;database=agile-project;user=root;password=agileavengers");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
    }
}