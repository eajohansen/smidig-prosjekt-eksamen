using System.ComponentModel.DataAnnotations;

namespace agile_dev.Models;

public class SessionDateTime {
    
    /*
       Data annotations

       [KEY] = Data annotation for primary key of this model
       [Required] = Data annotation for making it a necessary field for the row
       [Display(Name = "*name*")] = Data annotation for which name is showing in when one looks at the database
       [DataType(*Type*)] = Data annotation for specifying the type of data
       [DisplayFormat(*format*)] = Data annotation for specifying the format of the data when displayed
       [DisplayFormat(*apply format in edit mode*)] = Data annotation for using the format in edit mode

    */

    public SessionDateTime() {
        
    }
    
    [Key] // Data annotation for primary key of this model
    [Display(Name = "Session DateTime Id")]
    public int SessionDateTimeId { get; set; }
    
    [Required]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
    [Display(Name = "Expire at")]
    public DateTime ExpireAt { get; set; }
    
    [Required]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
    [Display(Name = "Created at")]
    public DateTime CreatedAt { get; set; }
    
    [Required]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
    [Display(Name = "Updated at")]
    public DateTime UpdatedAt { get; set; }
    
    // The Session connected to this SessionDateTime
    public virtual Session Session { get; set; }
}