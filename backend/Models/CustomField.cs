using System.ComponentModel.DataAnnotations;

namespace agile_dev.Models;

public class CustomField {

    public CustomField() {
        
    }
    
    [Required]
    [Key]
    [Display(Name = "Custom field Id")]
    public int CustomFieldId { get; set; }
    
    [Required]
    [Display(Name = "Description")]
    [StringLength(200)]
    public string Name { get; set; }
    
    [Required]
    [Display(Name = "Value")]
    public bool Value { get; set; }
}