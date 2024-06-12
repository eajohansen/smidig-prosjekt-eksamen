using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace agile_dev.Models;

public class Organizer {
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Display(Name = "Organizer Id")]
    public int OrganizerId { get; set; }
    
    [Required]
    [Display(Name = "Organization id")]
    [ForeignKey("OrganizationId")]
    public int OrganizationId { get; set; }
    
    // Navigation properties
    public virtual User User { get; set; }
    
    [Required]
    [Display(Name = "User Id")]
    [ForeignKey("User")]
    public string UserId { get; set; } // Assuming UserId is a string as per IdentityUser
}