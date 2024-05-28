using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace agile_dev.Models;

public class User {

    public User() {
        
    }
    
    [Required]
    [Key]
    [Display(Name = "User Id")]
    public int UserId { get; set; }
    
    [Required]
    [Display(Name = "Email")]
    [StringLength(200)]
    public string Email { get; set; }
    
    [Required]
    [Display(Name = "Password")]
    [StringLength(200)]
    public string Password { get; set; }
    
    [Required]
    [Display(Name = "Admin rights")]
    public bool Admin { get; set; }
    
    [Required]
    [Display(Name = "Profile id")]
    public int ProfileId { get; set; }
    
    [ForeignKey("ProfileId")]
    public Profile Profile { get; set; }
    
    // Still need to see the reason
    [Required]
    [Display(Name = "User Event id")]
    public int UserEventId { get; set; }
    
    [ForeignKey("UserEventId")]
    public UserEvent UserEvent { get; set; }
    
    // Think this is a many to many
    [Required]
    [Display(Name = "Organization id")]
    public int OrganizationId { get; set; }
    
    [ForeignKey("OrganizationId")]
    public Organization Organization { get; set; }
    
    // Think this is a many to many
    [Required]
    [Display(Name = "Follow organization id")]
    public int FollowOrganizationId { get; set; }
    
    [ForeignKey("OrganizationId")]
    public Organization FollowOrganization { get; set; }
}