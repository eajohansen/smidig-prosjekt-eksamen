using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using agile_dev.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace agile_dev.Repo;

public class InitContext : IdentityDbContext<User> {
    private readonly IConfiguration _configuration;
    
    // Constructor that accepts DbContextOptions<InitContext>
    public InitContext(DbContextOptions<InitContext> options, IConfiguration configuration) 
        : base(options)
    {
        _configuration = configuration;
    }
  
    public InitContext() {
        _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
    }
    public DbSet<User> User { get; set; }
    public DbSet<Allergy> Allergy { get; set; }
    public DbSet<ContactPerson> ContactPerson { get; set; }
    public DbSet<Event> Event { get; set; }
    public DbSet<EventCustomField> EventCustomField { get; set; }
    public DbSet<Follower> Follower { get; set; }
    public DbSet<Image> Image { get; set; }
    public DbSet<CustomField> CustomField { get; set; }
    public DbSet<Notice> Notice { get; set; }
    public DbSet<Organization> Organization { get; set; }
    public DbSet<Organizer> Organizer { get; set; }    
    public DbSet<Place> Place { get; set; }
    public DbSet<UserEvent> UserEvent { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Organizer>()
            .HasOne(o => o.User)
            .WithMany(u => u.OrganizerOrganization)
            .HasForeignKey(o => o.UserId);
        
        modelBuilder.Entity<Follower>()
            .HasOne(o => o.User)
            .WithMany(u => u.FollowOrganization)
            .HasForeignKey(o => o.UserId);
        
        modelBuilder.Entity<UserEvent>()
            .HasOne(userEvent => userEvent.User)
            .WithMany(user => user.UserEvents)
            .HasForeignKey(userEvent => userEvent.Id);
        
        modelBuilder.Entity<UserEvent>()
            .HasOne(userEvent => userEvent.Event)
            .WithMany(eEvent => eEvent.UserEvents)
            .HasForeignKey(userEvent => userEvent.EventId);
        
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        if (!optionsBuilder.IsConfigured) {
            optionsBuilder.UseMySQL("Server=database,9999;Database=agile-project;User=root;Password=agileavengers;");
        }
    }
}