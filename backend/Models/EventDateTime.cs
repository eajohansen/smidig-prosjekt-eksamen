using System.ComponentModel.DataAnnotations;

namespace agile_dev.Models;

public class EventDateTime {
    
    /*
       Data annotations

       [KEY] = Data annotation for primary key of this model
       [Required] = Data annotation for making it a necessary field for the row
       [Display(Name = "*name*")] = Data annotation for which name is showing in when one looks at the database
       [DataType(*Type*)] = Data annotation for specifying the type of data
       [DisplayFormat(*format*)] = Data annotation for specifying the format of the data when displayed
       [DisplayFormat(*apply format in edit mode*)] = Data annotation for using the format in edit mode

    */

    public EventDateTime() {
        
    }
    
    [Key] // Data annotation for primary key of this model
    [Display(Name = "Event DateTime Id")]
    public int EventDateTimeId { get; set; }
    
    [Required]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
    [Display(Name = "Created at")]
    public DateTime CreatedAt { get; set; }
    
    [Required]
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