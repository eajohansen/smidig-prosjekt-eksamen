using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace agile_dev.Models;

public class UserEvent {

    public UserEvent() {
        
    }
    
    [Required]
    [Key]
    [Display(Name = "User Event Id")]
    public int UserEventId { get; set; }
    
    [Display(Name = "Queue Number")]
    public int QueueNumber { get; set; }
    
    [Required]
    [Display(Name = "User id")]
    public int UserId { get; set; }
    
    [ForeignKey("Userid")]
    public User User { get; set; }
    
    [Required]
    [Display(Name = "Event id")]
    public int EventId { get; set; }
    
    [ForeignKey("EventId")]
    public Event Event { get; set; }
}