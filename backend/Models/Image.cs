using System.ComponentModel.DataAnnotations;

namespace agile_dev.Models;

public class Image {

    public Image() {
        
    }
    
    [Required]
    [Key]
    [Display(Name = "Image Id")]
    public int ImageId { get; set; }
    
    [Required]
    [Display(Name = "Link")]
    [StringLength(200)]
    public string Link { get; set; }
}