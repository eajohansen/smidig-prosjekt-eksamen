using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using agile_dev.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace agile_dev.Repo;

public class InitContext : IdentityDbContext<IdentityUser> {
    public InitContext(DbContextOptions<InitContext> options) : base(options) {
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
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        if (!optionsBuilder.IsConfigured) {
            optionsBuilder.UseMySQL("Server=database,9999;Database=agile-project;User=root;Password=agileavengers;Max Pool Size=1024;Connect Timeout=10;");
        }
    }
}