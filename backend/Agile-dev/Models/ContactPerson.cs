using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace agile_dev.Models;

public class ContactPerson {
    
    /*
       Data annotations

       [KEY] = Data annotation for primary key of this model
       [Required] = Data annotation for making it a necessary field for the row
       [Display(Name = "*name*")] = Data annotation for which name is showing in when one looks at the database
       [StringLength(*number*)] = Data annotation for setting a max length on the field
       [DataType(*Type*)] = Data annotation for specifying the type of data
       [EmailAddress(ErrorMessage = "Invalid Email Address")] = Data annotation for specifying that this needs to be an email

    */
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Display(Name = "Contact person Id")]
    public int ContactPersonId { get; set; }
    
    [Required]
    [Display(Name = "Name")]
    [StringLength(200)]
    public string Name { get; set; }
    
    [Display(Name = "Email")]
    [StringLength(200)]
    [DataType(DataType.EmailAddress)]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string? Email { get; set; }
    
    [Display(Name = "Phone number")]
    [StringLength(200)]
    public string? Number { get; set; }
    
    // A HasSet of all Events with this ContactPerson
    [JsonIgnore]
    public ICollection<Event>? Events { get; set; }
}