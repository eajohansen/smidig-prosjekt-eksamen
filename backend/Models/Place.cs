using System.ComponentModel.DataAnnotations;

namespace agile_dev.Models;

public class Place {
    
    /*
       Data annotations

       [KEY] = Data annotation for primary key of this model
       [Required] = Data annotation for making it a necessary field for the row
       [Display(Name = "*name*")] = Data annotation for which name is showing in when one looks at the database
       [StringLength(*number*)] = Data annotation for setting a max length on the field

    */

    public Place() {
        // Initializing to avoid NULL reference
        Events = new HashSet<Event>();
    }
    
    [Key]
    [Display(Name = "Place Id")]
    public int PlaceId { get; set; }
    
    [Required]
    [Display(Name = "Location")]
    [StringLength(200)]
    public string Location { get; set; }
    
    [Display(Name = "Url")]
    [StringLength(500)]
    public string Url { get; set; }
    
    public virtual ICollection<Event> Events { get; set; }
}