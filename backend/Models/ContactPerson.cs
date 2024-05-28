using System.ComponentModel.DataAnnotations;

namespace agile_dev.Models;

public class ContactPerson {

    public ContactPerson() {
        
    }
    
    [Required]
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
    [StringLength(200)]
    public string Number { get; set; }
}