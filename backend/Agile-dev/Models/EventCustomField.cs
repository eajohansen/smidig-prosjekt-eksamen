using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace agile_dev.Models;

public class EventCustomField {
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Display(Name = "Event Custom field id")]
    public int EventCustomFieldId { get; set; }
    
    [Display(Name = "Custom field id")]
    [ForeignKey("CustomFieldId")]
    public int? CustomFieldId { get; set; }
    public CustomField? CustomField { get; set; }
    
    [Display(Name = "Event id")]
    [ForeignKey("EventId")]
    public int? EventId { get; set; }
}