using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace agile_dev.Models;

public class User {
    
    /*
       Data annotations

       [KEY] = Data annotation for primary key of this model
       [Required] = Data annotation for making it a necessary field for the row
       [Display(Name = "*name*")] = Data annotation for which name is showing in when one looks at the database
       [StringLength(*number*)] = Data annotation for setting a max length on the field
       [DataType(*Type*)] = Data annotation for specifying the type of data
       [DisplayFormat(*format*)] = Data annotation for specifying the format of the data when displayed
       [DisplayFormat(*apply format in edit mode*)] = Data annotation for using the format in edit mode
       [EmailAddress(ErrorMessage = "Invalid Email Address")] = Data annotation for specifying that this needs to be an email
    */

    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Display(Name = "User Id")]
    public int UserId { get; set; }
    
    [Required]
    [Display(Name = "Email")]
    [StringLength(200)]
    [DataType(DataType.EmailAddress)]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string Email { get; set; }
    
    [Required]
    [Display(Name = "Admin rights")]
    public bool Admin { get; set; }
    
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
    public DateTime? Birthdate { get; set; }
    
    [Display(Name = "Extra info")]
    [StringLength(1000)]
    public string? ExtraInfo { get; set; }
    
    // A HasSet of all Organizations that this User follows
    public ICollection<Follower>? FollowOrganization { get; set; }
    
    // A HasSet of all Organizations that this User can organize
    public ICollection<Organizer>? OrganizerOrganization { get; set; }
    
    // A HasSet of all UserEvents with this User 
    public ICollection<UserEvent>? UserEvents { get; set; }
    
    // A HasSet of all Notices with this User
    public ICollection<Notice>? Notices { get; set; }
    
    // A HasSet of all Allergies with this User
    public ICollection<Allergy>? Allergies { get; set; }
}