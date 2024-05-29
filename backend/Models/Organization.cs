using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace agile_dev.Models;

public class Organization {
    
    /*
       Data annotations

       [KEY] = Data annotation for primary key of this model
       [Required] = Data annotation for making it a necessary field for the row
       [Display(Name = "*name*")] = Data annotation for which name is showing in when one looks at the database
       [StringLength(*number*)] = Data annotation for setting a max length on the field
       [ForeignKey("*key*")] = Data annotation for choosing which element to connect up a relationship

    */

    public Organization() {
        // Initializing them to avoid NULL reference
        Followers = new HashSet<Follower>();
        Organisators = new HashSet<Organisator>();
    }
    
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
    public virtual Image Image { get; set; }
    
    // A HasSet of all Followers with this Organization
    public virtual ICollection<Follower> Followers { get; set; }
    
    // A HasSet of all Organisators with this Organization
    public virtual ICollection<Organisator> Organisators { get; set; }
}