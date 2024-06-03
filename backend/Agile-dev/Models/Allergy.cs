using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace agile_dev.Models;

public class Allergy {
     
     /*
       Data annotations

       [KEY] = Data annotation for primary key of this model
       [Required] = Data annotation for making it a necessary field for the row
       [Display(Name = "*name*")] = Data annotation for which name is showing in when one looks at the database
       [StringLength(*number*)] = Data annotation for setting a max length on the field
       [ForeignKey("*key*")] = Data annotation for choosing which element to connect up a relationship
       [DatabaseGenerated(DatabaseGeneratedOption.Identity)] = This specific data annotation gives this model a private counter for id 

    */

     public Allergy() {
     }
     
     [Key]
     [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
     [Display(Name = "Allergy Id")]
     public int AllergyId { get; set; }
     
     [Required]
     [Display(Name = "Name")]
     [StringLength(200)]
     public string Name { get; set; }
     
     [Display(Name = "Description")]
     [StringLength(1000)]
     public string? Description { get; set; }
     
     [Required]
     [Display(Name = "User id")]
     public int UserId { get; set; }
    
     [ForeignKey("UserId")]
     public virtual User User { get; set; }
}