using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace agile_dev.Models;

public class Allergy {

     public Allergy() {
          
     }
     
     [Required]
     [Key]
     [Display(Name = "Allergy Id")]
     public int AllergyId { get; set; }
     
     [Required]
     [Display(Name = "Name")]
     [StringLength(200)]
     public string Name { get; set; }
     
     [Required]
     [Display(Name = "Description")]
     [StringLength(200)]
     public string Description { get; set; }
     
     // Still need to see the reason
     [Required]
     [Display(Name = "Profile Allergy id")]
     public int ProfileAllergyId { get; set; }
    
     [ForeignKey("ProfileAllergyId")]
     public ProfileAllergy ProfileAllergy { get; set; }
}