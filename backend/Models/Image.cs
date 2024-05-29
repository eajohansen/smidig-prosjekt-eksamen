using System.ComponentModel.DataAnnotations;

namespace agile_dev.Models;

public class Image {
    
    /*
       Data annotations

       [KEY] = Data annotation for primary key of this model
       [Required] = Data annotation for making it a necessary field for the row
       [Display(Name = "*name*")] = Data annotation for which name is showing in when one looks at the database
       [StringLength(*number*)] = Data annotation for setting a max length on the field
       
    */

    public Image() {
        // Initializing them to avoid NULL reference
        Organizations = new HashSet<Organization>();
        Events = new HashSet<Event>();
    }
    
    [Key] // Data annotation for primary key of this model
    [Display(Name = "Image Id")]
    public int ImageId { get; set; }
    
    [Required]
    [Display(Name = "Link")]
    [StringLength(500)]
    public string Link { get; set; }
    
    public virtual ICollection<Organization> Organizations { get; set; }
    
    public virtual ICollection<Event> Events { get; set; }
}