using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace agile_dev.Models;

public class Profile {
    
    /*
       Data annotations

       [KEY] = Data annotation for primary key of this model
       [Required] = Data annotation for making it a necessary field for the row
       [Display(Name = "*name*")] = Data annotation for which name is showing in when one looks at the database
       [StringLength(*number*)] = Data annotation for setting a max length on the field
       [DataType(*Type*)] = Data annotation for specifying the type of data
       [DisplayFormat(*format*)] = Data annotation for specifying the format of the data when displayed
       [DisplayFormat(*apply format in edit mode*)] = Data annotation for using the format in edit mode
       [DatabaseGenerated(DatabaseGeneratedOption.Identity)] = This specific data annotation gives this model a private counter for id

    */

    public Profile() {
        Allergies = new HashSet<Allergy>();
    }
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
    
    [Display(Name = "Extra info")]
    [StringLength(1000)]
    public string ExtraInfo { get; set; }
    
    // A HasSet of all Allergies with this Profile
    public virtual ICollection<Allergy> Allergies { get; set; }
}