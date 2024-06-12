using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace agile_dev.Models;

public class UserEvent {
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Display(Name = "User Event Id")]
    public int UserEventId { get; set; }
    
    [Display(Name = "Used")]
    public bool Used { get; set; }
    
    [Display(Name = "Queue Number")]
    public int QueueNumber { get; set; }
    
    [Required]
    [Display(Name = "User id")]
    [ForeignKey("Id")]
    public string Id { get; set; }
    
    [Required]
    [Display(Name = "Event id")]
    [ForeignKey("EventId")]
    public int EventId { get; set; }
    
    // Navigation properties
    public User User { get; set; }
    public Event Event { get; set; }
}