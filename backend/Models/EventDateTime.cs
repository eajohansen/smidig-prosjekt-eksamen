using System.ComponentModel.DataAnnotations;

namespace agile_dev.Models;

public class EventDateTime {

    public EventDateTime() {
        
    }
    
    [Key] // Data annotation for primary key of this model
    [Display(Name = "Event DateTime Id")]
    public int EventDateTimeId { get; set; }
    
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
    [Display(Name = "Created at")]
    public DateTime CreatedAt { get; set; }
    
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
    [Display(Name = "Start time")]
    public DateTime StartTime { get; set; }
    
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
    [Display(Name = "End time")]
    public DateTime EndTime { get; set; }
    
    public virtual Event Event { get; set; }
}