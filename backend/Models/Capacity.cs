using System.ComponentModel.DataAnnotations;

namespace agile_dev.Models;

public class Capacity {

    public Capacity() {
        
    }
    
    [Required]
    [Key]
    [Display(Name = "Capacity id")]
    public int CapacityId { get; set; }
    
    [Required]
    [Display(Name = "Capacity")]
    public int MaxParticipants { get; set; }
}