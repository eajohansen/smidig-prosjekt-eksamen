using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace agile_dev.Models;

public class User : IdentityUser {
    
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
    
    [Display(Name = "First name")]
    [StringLength(200)]
    public string? FirstName { get; set; }
    
    [Display(Name = "Last name")]
    [StringLength(200)]
    public string? LastName { get; set; }
    
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
    [Display(Name = "Birthdate")]
    public DateTime? Birthdate { get; set; }
    
    [Display(Name = "Extra info")]
    [StringLength(1000)]
    public string? ExtraInfo { get; set; }
    
    // A HasSet of all Organizations that this User follows
    [JsonIgnore]
    public ICollection<Follower>? FollowOrganization { get; set; }
    
    // A HasSet of all Organizations that this User can organize
    [JsonIgnore]
    public ICollection<Organizer>? OrganizerOrganization { get; set; }
    
    // A HasSet of all UserEvents with this User 
    [JsonIgnore]
    public ICollection<UserEvent>? UserEvents { get; set; }
    
    // A HasSet of all Notices with this User
    [JsonIgnore]
    public ICollection<Notice>? Notices { get; set; }
    
    // A HasSet of all Allergies with this User
    [JsonIgnore]
    public ICollection<Allergy>? Allergies { get; set; }
}