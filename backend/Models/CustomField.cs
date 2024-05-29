using System.ComponentModel.DataAnnotations;

namespace agile_dev.Models;

public class CustomField {
    
    /*
       Data annotations

       [KEY] = Data annotation for primary key of this model
       [Required] = Data annotation for making it a necessary field for the row
       [Display(Name = "*name*")] = Data annotation for which name is showing in when one looks at the database
       [StringLength(*number*)] = Data annotation for setting a max length on the field

    */

    public CustomField() {
        // Initializing them to avoid NULL reference
        EventCustomFields = new HashSet<EventCustomField>();
    }
    
    [Key]
    [Display(Name = "Custom field Id")]
    public int CustomFieldId { get; set; }
    
    [Required]
    [Display(Name = "Description")]
    [StringLength(200)]
    public string Description { get; set; }
    
    [Required]
    [Display(Name = "Value")]
    public bool Value { get; set; }
    
    // A HasSet of all EventCustomFields with this CustomField
    public virtual ICollection<EventCustomField> EventCustomFields { get; set; }
}