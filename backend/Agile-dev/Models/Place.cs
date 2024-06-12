using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace agile_dev.Models;

public class Place {
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Display(Name = "Place Id")]
    public int PlaceId { get; set; }
    
    [Required]
    [Display(Name = "Location")]
    [StringLength(200)]
    public string Location { get; set; }
    
    [Display(Name = "Url")]
    [StringLength(500)]
    public string? Url { get; set; }
    
    // A HasSet of all Events with this Place
    [JsonIgnore]
    public ICollection<Event>? Events { get; set; }
}