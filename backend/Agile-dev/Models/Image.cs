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

    */

    public Image(string link) {
        Link = link;
    }
    
    [Key] // Data annotation for primary key of this model
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
    public ICollection<Organization>? Organizations { get; set; }
    
    // A HasSet of all Events with this Image
    public ICollection<Event>? Events { get; set; }
}