using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace agile_dev.Models;

public class ContactPerson {
    
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