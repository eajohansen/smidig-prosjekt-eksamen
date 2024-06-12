using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace agile_dev.Models;

public class CustomField {
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
    [JsonIgnore]
    public ICollection<EventCustomField>? EventCustomFields { get; set; }
}