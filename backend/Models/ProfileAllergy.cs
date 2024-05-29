using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace agile_dev.Models;

public class ProfileAllergy {
    
    /*
       Data annotations

       [KEY] = Data annotation for primary key of this model
       [Required] = Data annotation for making it a necessary field for the row
       [Display(Name = "*name*")] = Data annotation for which name is showing in when one looks at the database
       [StringLength(*number*)] = Data annotation for setting a max length on the field
       [ForeignKey("*key*")] = Data annotation for choosing which element to connect up a relationship

    */

    public ProfileAllergy() {
        
    }
    
    [Key] // Data annotation for primary key of this model
    [Display(Name = "Profile Allergy Id")]
    public int ProfileAllergyId { get; set; }
    
    [Required]
    [Display(Name = "Profile id")]
    public int ProfileId { get; set; }
    
    [ForeignKey("ProfileId")]
    public virtual Profile Profile { get; set; }
    
    [Required]
    [Display(Name = "Allergy id")]
    public int AllergyId { get; set; }
    
    [ForeignKey("AllergyId")]
    public virtual Allergy Allergy { get; set; }
}