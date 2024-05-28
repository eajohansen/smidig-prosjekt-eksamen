using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace agile_dev.Models;

public class Session {

    public Session() {
        
    }
    
    [Required]
    [Key]
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
    public SessionDateTime SessionDateTime { get; set; }
}