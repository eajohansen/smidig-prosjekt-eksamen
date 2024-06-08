using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace agile_dev.Models;

public class Organizer {
    
    /*
       Data annotations

       [KEY] = Data annotation for primary key of this model
       [Required] = Data annotation for making it a necessary field for the row
       [Display(Name = "*name*")] = Data annotation for which name is showing in when one looks at the database
       [StringLength(*number*)] = Data annotation for setting a max length on the field
       [ForeignKey("*key*")] = Data annotation for choosing which element to connect up a relationship

    */
    
    [Key] // Data annotation for primary key of this model
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Display(Name = "Organizer Id")]
    public int OrganizerId { get; set; }
    
    [Required]
    [Display(Name = "Organization id")]
    [ForeignKey("OrganizationId")]
    public int OrganizationId { get; set; }
    
    [Required]
    [Display(Name = "User id")]
    [ForeignKey("Id")]
    public string Id { get; set; }
}