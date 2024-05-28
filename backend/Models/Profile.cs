using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace agile_dev.Models;

public class Profile {

    public Profile() {
        
    }
    
    [Required]
    [Key]
    [Display(Name = "Profile Id")]
    public int ProfileId { get; set; }
    
    [Required]
    [Display(Name = "First name")]
    [StringLength(200)]
    public string FirstName { get; set; }
    
    [Required]
    [Display(Name = "Last name")]
    [StringLength(200)]
    public string LastName { get; set; }
    
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [Display(Name = "Birthdate")]
    public DateTime Birthdate { get; set; }
    
    // Still need to see the reason
    [Required]
    [Display(Name = "Profile Allergy id")]
    public int ProfileAllergyId { get; set; }
    
    [ForeignKey("ProfileAllergyId")]
    public ProfileAllergy ProfileAllergy { get; set; }
}