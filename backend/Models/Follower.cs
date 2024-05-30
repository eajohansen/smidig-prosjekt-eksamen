using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace agile_dev.Models;

public class Follower {
    
    /*
       Data annotations

       [KEY] = Data annotation for primary key of this model
       [Required] = Data annotation for making it a necessary field for the row
       [Display(Name = "*name*")] = Data annotation for which name is showing in when one looks at the database
       [ForeignKey("*key*")] = Data annotation for choosing which element to connect up a relationship

    */

    public Follower() {
        
    }
    
    [Key] // Data annotation for primary key of this model
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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