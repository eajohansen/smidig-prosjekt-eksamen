using System.ComponentModel.DataAnnotations;

namespace agile_dev.Models;

public class Capacity {
    
    /*
       Data annotations

       [KEY] = Data annotation for primary key of this model
       [Required] = Data annotation for making it a necessary field for the row
       [Display(Name = "*name*")] = Data annotation for which name is showing in when one looks at the database

    */

    public Capacity() {
        // Initializing to avoid NULL reference
        Events = new HashSet<Event>();
    }
    
    [Key]
    [Display(Name = "Capacity id")]
    public int CapacityId { get; set; }
    
    [Required]
    [Display(Name = "MaxParticipants")]
    public int MaxParticipants { get; set; }
    
    public virtual ICollection<Event> Events { get; set; }
}