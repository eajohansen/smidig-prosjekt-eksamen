using System.ComponentModel.DataAnnotations;

namespace agile_dev.Models;

public class EventDateTime {

    public EventDateTime() {
        
    }
    
    [Required]
    [Key]
    [Display(Name = "Event DateTime Id")]
    public int EventDateTimeId { get; set; }
    
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
    [Display(Name = "Created at")]
    public DateTime CreatedAt { get; set; }
    
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
    [Display(Name = "Start time")]
    public DateTime StartTime { get; set; }
    
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
    [Display(Name = "End time")]
    public DateTime EndTime { get; set; }
}