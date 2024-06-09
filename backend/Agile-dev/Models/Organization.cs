using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace agile_dev.Models;

public class Organization {
    
    /*
       Data annotations

       [KEY] = Data annotation for primary key of this model
       [Required] = Data annotation for making it a necessary field for the row
       [Display(Name = "*name*")] = Data annotation for which name is showing in when one looks at the database
       [StringLength(*number*)] = Data annotation for setting a max length on the field
       [ForeignKey("*key*")] = Data annotation for choosing which element to connect up a relationship

    */
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Display(Name = "Organization Id")]
    public int OrganizationId { get; set; }
    
    [Required]
    [Display(Name = "Name")]
    [StringLength(200)]
    public string Name { get; set; }
    
    [Display(Name = "Description")]
    [StringLength(2000)]
    public string? Description { get; set; }
    
    [Display(Name = "Image id")]
    [ForeignKey("ImageId")]
    public int? ImageId { get; set; }
    public Image? Image { get; set; }
    
    // A HasSet of all Followers with this Organization
    [JsonIgnore]
    public ICollection<Follower>? Followers { get; set; }
    
    // A HasSet of all Organizers with this Organization
    [JsonIgnore]
    public ICollection<Organizer>? Organizers { get; set; }
    
    // A HasSet of all Events with this Organization
    [JsonIgnore]
    public ICollection<Event>? Events { get; set; }
}