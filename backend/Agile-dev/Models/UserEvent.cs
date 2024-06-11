using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace agile_dev.Models;

public class UserEvent {
    
    /*
       Data annotations

       [KEY] = Data annotation for primary key of this model
       [Required] = Data annotation for making it a necessary field for the row
       [Display(Name = "*name*")] = Data annotation for which name is showing in when one looks at the database
       [ForeignKey("*key*")] = Data annotation for choosing which element to connect up a relationship

    */
    
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