using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace agile_dev.Models;

public class Allergy {
     
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Display(Name = "Allergy Id")]
    public int AllergyId { get; set; }
     
    [Required]
    [Display(Name = "Name")]
    [StringLength(200)]
    public string Name { get; set; }
     
    [Display(Name = "Description")]
    [StringLength(1000)]
    public string? Description { get; set; }

}