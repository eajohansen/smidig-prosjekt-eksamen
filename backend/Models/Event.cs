using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace agile_dev.Models;

public class Event {

    public Event() {
        
    }
    
    [Required]
    [Key]
    [Display(Name = "Event Id")]
    public int EventId { get; set; }
    
    [Required]
    [Display(Name = "Title")]
    [StringLength(200)]
    public string Title { get; set; }
    
    [Required]
    [Display(Name = "Description")]
    [StringLength(200)]
    public string Description { get; set; }
    
    [Required]
    [Display(Name = "Event DateTime id")]
    public int EventDateTimeId { get; set; }
    
    [ForeignKey("EventDateTimeId")]
    public EventDateTime EventDateTime { get; set; }
    
    [Required]
    [Display(Name = "Capacity id")]
    public int CapacityId { get; set; }
    
    [ForeignKey("CapacityId")]
    public Capacity Capacity { get; set; }
    
    [Required]
    [Display(Name = "Place id")]
    public int PlaceId { get; set; }
    
    [ForeignKey("PlaceId")]
    public Place Place { get; set; }
    
    // Still need to see the reason
    [Required]
    [Display(Name = "User Event id")]
    public int UserEventId { get; set; }
    
    [ForeignKey("UserEventId")]
    public UserEvent UserEvent { get; set; }
    
    // Still need to see the reason
    [Required]
    [Display(Name = "Event Custom field id")]
    public int EventCustomFieldId { get; set; }
    
    [ForeignKey("EventCustomFileId")]
    public EventCustomField EventCustomField { get; set; }
    
    [Required]
    [Display(Name = "Image id")]
    public int ImageId { get; set; }
    
    [ForeignKey("ImageId")]
    public Image Image { get; set; }
    
    [Required]
    [Display(Name = "Contact person id")]
    public int ContactPersonId { get; set; }
    
    [ForeignKey("ContactPersonId")]
    public ContactPerson ContactPerson { get; set; }
}