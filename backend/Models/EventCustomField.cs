using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace agile_dev.Models;

public class EventCustomField {

    public EventCustomField() {
        
    }
    
    [Required]
    [Key]
    [Display(Name = "Event Custom field id")]
    public int EventCustomFieldId { get; set; }
    
    [Required]
    [Display(Name = "Custom field id")]
    public int CustomFieldId { get; set; }
    
    [ForeignKey("CustomFieldId")]
    public CustomField CustomField { get; set; }
    
    [Required]
    [Display(Name = "Event id")]
    public int EventId { get; set; }
    
    [ForeignKey("EventId")]
    public Event Event { get; set; }
}