using System.ComponentModel.DataAnnotations;

namespace agile_dev.Models;

public class SessionDateTime {

    public SessionDateTime() {
        
    }
    
    [Required]
    [Key]
    [Display(Name = "Session DateTime Id")]
    public int SessionDateTimeId { get; set; }
    
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
    [Display(Name = "Expire at")]
    public DateTime ExpireAt { get; set; }
    
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
    [Display(Name = "Created at")]
    public DateTime CreatedAt { get; set; }
    
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
    [Display(Name = "Updated at")]
    public DateTime UpdatedAt { get; set; }
}