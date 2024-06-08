using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace agile_dev.Models;

public class CustomField {
    
    /*
       Data annotations

       [KEY] = Data annotation for primary key of this model
       [Required] = Data annotation for making it a necessary field for the row
       [Display(Name = "*name*")] = Data annotation for which name is showing in when one looks at the database
       [StringLength(*number*)] = Data annotation for setting a max length on the field

    */
    
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