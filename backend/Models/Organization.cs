using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace agile_dev.Models;

public class Organization {

    public Organization() {
        
    }
    
    [Required]
    [Key]
    [Display(Name = "Organization Id")]
    public int OrganizationId { get; set; }
    
    [Required]
    [Display(Name = "Name")]
    [StringLength(200)]
    public string Name { get; set; }
    
    [Required]
    [Display(Name = "Description")]
    [StringLength(2000)]
    public string Description { get; set; }
    
    [Required]
    [Display(Name = "Image id")]
    public int ImageId { get; set; }
    
    [ForeignKey("ImageId")]
    public Image Image { get; set; }
    
    // Still need to see the reason
    [Required]
    [Display(Name = "Organisator id")]
    public int OrganisatorId { get; set; }
    
    [ForeignKey("UserId")]
    public User Organisator { get; set; }
    
    // Still need to see the reason
    [Required]
    [Display(Name = "Follower id")]
    public int FollowerId { get; set; }
    
    [ForeignKey("UserId")]
    public User Follower { get; set; }
}