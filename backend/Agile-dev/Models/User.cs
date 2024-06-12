using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace agile_dev.Models;

public class User : IdentityUser {
    
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