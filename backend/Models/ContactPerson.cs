using System.ComponentModel.DataAnnotations;

namespace agile_dev.Models;

public class ContactPerson {
    
    /*
       Data annotations

       [KEY] = Data annotation for primary key of this model
       [Required] = Data annotation for making it a necessary field for the row
       [Display(Name = "*name*")] = Data annotation for which name is showing in when one looks at the database
       [StringLength(*number*)] = Data annotation for setting a max length on the field
       
    */

    public ContactPerson() {
        // Initializing to avoid NULL reference
        Events = new HashSet<Event>();
    }
    
    [Key]
    [Display(Name = "Contact person Id")]
    public int ContactPersonId { get; set; }
    
    [Required]
    [Display(Name = "Name")]
    [StringLength(200)]
    public string Name { get; set; }
    
    [Required]
    [Display(Name = "Email")]
    [StringLength(200)]
    public string Email { get; set; }
    
    [Required]
    [Display(Name = "Phone number")]
    public int Number { get; set; }
    
    // 
    public virtual ICollection<Event> Events { get; set; }
}