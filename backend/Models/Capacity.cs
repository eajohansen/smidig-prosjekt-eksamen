using System.ComponentModel.DataAnnotations;

namespace agile_dev.Models;

public class Capacity {

    public Capacity() {
        // Initializing to avoid NULL reference
        Events = new HashSet<Event>();
    }
    
    [Key] // Data annotation for primary key of this model
    [Display(Name = "Capacity id")]
    public int CapacityId { get; set; }
    
    [Required]
    [Display(Name = "MaxParticipants")]
    public int MaxParticipants { get; set; }
    
    public virtual ICollection<Event> Events { get; set; }
}