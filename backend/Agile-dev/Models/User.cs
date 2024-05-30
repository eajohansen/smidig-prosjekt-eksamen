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
       [ForeignKey("*key*")] = Data annotation for choosing which element to connect up a relationship
       [DataType(*Type*)] = Data annotation for specifying the type of data
       [EmailAddress(ErrorMessage = "Invalid Email Address")] = Data annotation for specifying that this needs to be an email
       [DatabaseGenerated(DatabaseGeneratedOption.Identity)] = This specific data annotation gives this model a private counter for id

    */

    public User() {
        // Initializing them to avoid NULL reference
        FollowOrganization = new HashSet<Follower>();
        OrganisatorOrganization = new HashSet<Organisator>();
        UserEvents = new HashSet<UserEvent>();
    }
    
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
    [Display(Name = "Password")]
    [StringLength(200)]
    public string Password { get; set; }
    
    [Required]
    [Display(Name = "Admin rights")]
    public bool Admin { get; set; }
    
    [Display(Name = "Profile id")]
    public int? ProfileId { get; set; }

    [ForeignKey("ProfileId")] 
    public virtual Profile? Profile { get; set; }
    
    // A HasSet of all Organizations that this User follows
    public virtual ICollection<Follower>? FollowOrganization { get; set; }
    
    // A HasSet of all Organizations that this User can organize
    public virtual ICollection<Organisator>? OrganisatorOrganization { get; set; }
    
    // A HasSet of all UserEvents with this User 
    public virtual ICollection<UserEvent>? UserEvents { get; set; }
    
    // A HasSet of all Notices with this User
    public virtual ICollection<Notice>? Notices { get; set; }
}