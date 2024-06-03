using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace agile_dev.Models;

public class EventCustomField {
    
    /*
       Data annotations

       [KEY] = Data annotation for primary key of this model
       [Required] = Data annotation for making it a necessary field for the row
       [Display(Name = "*name*")] = Data annotation for which name is showing in when one looks at the database
       [ForeignKey("*key*")] = Data annotation for choosing which element to connect up a relationship
       [DatabaseGenerated(DatabaseGeneratedOption.Identity)] = This specific data annotation gives this model a private counter for id

    */

    public EventCustomField() {
        
    }
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Display(Name = "Event Custom field id")]
    public int EventCustomFieldId { get; set; }
    
    [Required]
    [Display(Name = "Custom field id")]
    public int CustomFieldId { get; set; }
    
    [ForeignKey("CustomFieldId")]
    public virtual CustomField CustomField { get; set; }
    
    [Required]
    [Display(Name = "Event id")]
    public int EventId { get; set; }
    
    [ForeignKey("EventId")]
    public virtual Event Event { get; set; }
}