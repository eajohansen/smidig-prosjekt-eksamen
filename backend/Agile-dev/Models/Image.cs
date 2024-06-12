using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace agile_dev.Models;

public class Image {
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Display(Name = "Image Id")]
    public int ImageId { get; set; }
    
    [Required]
    [Display(Name = "Link")]
    [StringLength(500)]
    public string Link { get; set; }
    
    [Display(Name = "Image description")]
    [StringLength(200)]
    public string? ImageDescription { get; set; }
    
    // A HasSet of all Organizations with this Image
    [JsonIgnore]
    public ICollection<Organization>? Organizations { get; set; }
    
    // A HasSet of all Events with this Image
    [JsonIgnore]
    public ICollection<Event>? Events { get; set; }
}