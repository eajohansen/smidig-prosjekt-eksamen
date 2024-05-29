using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace agile_dev.Models;

public class Follower {

    public Follower() {
        
    }
    
    [Key] // Data annotation for primary key of this model
    [Display(Name = "Follower Id")]
    public int FollowerId { get; set; }
    
    [Required]
    [Display(Name = "Organization id")]
    public int OrganizationId { get; set; }
    
    [ForeignKey("OrganizationId")]
    public virtual Organization Organization { get; set; }
    
    [Required]
    [Display(Name = "User id")]
    public int UserId { get; set; }
    
    [ForeignKey("UserId")]
    public virtual User User { get; set; }
}