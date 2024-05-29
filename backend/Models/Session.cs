using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace agile_dev.Models;

public class Session {
    
    /*
       Data annotations

       [KEY] = Data annotation for primary key of this model
       [Required] = Data annotation for making it a necessary field for the row
       [Display(Name = "*name*")] = Data annotation for which name is showing in when one looks at the database
       [StringLength(*number*)] = Data annotation for setting a max length on the field
       [ForeignKey("*key*")] = Data annotation for choosing which element to connect up a relationship

    */

    public Session() {
        
    }
    
    [Key] // Data annotation for primary key of this model
    [Display(Name = "Session Id")]
    public int SessionId { get; set; }
    
    [Required]
    [Display(Name = "Token")]
    [StringLength(200)]
    public string Token { get; set; }
    
    [Required]
    [Display(Name = "Valid")]
    public bool Valid { get; set; }
    
    [Required]
    [Display(Name = "Type")]
    [StringLength(200)]
    public string Type { get; set; }
    
    [Required]
    [Display(Name = "User agent")]
    [StringLength(200)]
    public string UserAgent { get; set; }
    
    [Required]
    [Display(Name = "Session DateTime id")]
    public int SessionDateTimeId { get; set; }
    
    [ForeignKey("SessionDateTimeId")]
    public virtual SessionDateTime SessionDateTime { get; set; }
}