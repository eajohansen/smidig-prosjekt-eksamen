using System.ComponentModel.DataAnnotations;

namespace agile_dev.Models;

public class Allergy {

     public Allergy() {
          // Initializing to avoid NULL reference
          ProfileAllergies = new HashSet<ProfileAllergy>();
     }
     
     [Key] // Data annotation for primary key of this model
     [Display(Name = "Allergy Id")]
     public int AllergyId { get; set; }
     
     [Required]
     [Display(Name = "Name")]
     [StringLength(200)]
     public string Name { get; set; }
     
     [Display(Name = "Description")]
     [StringLength(1000)]
     public string Description { get; set; }
     
     public virtual ICollection<ProfileAllergy> ProfileAllergies { get; set; }
}