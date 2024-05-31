using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace agile_dev.Models;

public class Image {
    
    /*
       Data annotations

       [KEY] = Data annotation for primary key of this model
       [Required] = Data annotation for making it a necessary field for the row
       [Display(Name = "*name*")] = Data annotation for which name is showing in when one looks at the database
       [StringLength(*number*)] = Data annotation for setting a max length on the field
       [DatabaseGenerated(DatabaseGeneratedOption.Identity)] = This specific data annotation gives this model a private counter for id
       
    */

    public Image() {
        // Initializing them to avoid NULL reference
        Organizations = new HashSet<Organization>();
        Events = new HashSet<Event>();
    }
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Display(Name = "Image Id")]
    public int ImageId { get; set; }
    
    [Required]
    [Display(Name = "Link")]
    [StringLength(500)]
    public string Link { get; set; }
    
    [Display(Name = "Image description")]
    [StringLength(200)]
    public string? ImageDescription { get; set; }
    
    // A HasSet of all Organizations with this Image
    public virtual ICollection<Organization> Organizations { get; set; }
    
    // A HasSet of all Events with this Image
    public virtual ICollection<Event> Events { get; set; }
}