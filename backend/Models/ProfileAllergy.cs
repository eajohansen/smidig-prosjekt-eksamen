using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace agile_dev.Models;

public class ProfileAllergy {

    public ProfileAllergy() {
        
    }
    
    [Required]
    [Key]
    [Display(Name = "Profile Allergy Id")]
    public int ProfileAllergyId { get; set; }
    
    [Required]
    [Display(Name = "Profile id")]
    public int ProfileId { get; set; }
    
    [ForeignKey("ProfileId")]
    public Profile Profile { get; set; }
    
    [Required]
    [Display(Name = "Allergy id")]
    public int AllergyId { get; set; }
    
    [ForeignKey("AllergyId")]
    public Allergy Allergy { get; set; }
}