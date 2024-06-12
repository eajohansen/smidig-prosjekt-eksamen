using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace agile_dev.Models;

public class Organization {
    
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