using System.ComponentModel.DataAnnotations;

namespace agile_dev.Models;

public class CustomField {

    public CustomField() {
        // Initializing them to avoid NULL reference
        EventCustomFields = new HashSet<EventCustomField>();
    }
    
    [Key] // Data annotation for primary key of this model
    [Display(Name = "Custom field Id")]
    public int CustomFieldId { get; set; }
    
    [Required]
    [Display(Name = "Description")]
    [StringLength(1000)]
    public string Description { get; set; }
    
    [Required]
    [Display(Name = "Value")]
    public bool Value { get; set; }
    
    public virtual ICollection<EventCustomField> EventCustomFields { get; set; }
}