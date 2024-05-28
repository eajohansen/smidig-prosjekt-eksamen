using System.ComponentModel.DataAnnotations;

namespace agile_dev.Models;

public class Place {

    public Place() {
        
    }
    
    [Required]
    [Key]
    [Display(Name = "Place Id")]
    public int PlaceId { get; set; }
    
    [Required]
    [Display(Name = "Street name")]
    [StringLength(200)]
    public string StreetName { get; set; }
    
    [Required]
    [Display(Name = "Street number")]
    public int StreetNumber { get; set; }
    
    [Required]
    [Display(Name = "City")]
    [StringLength(200)]
    public string City { get; set; }
    
    [Required]
    [Display(Name = "Postal code")]
    public int PostalCode { get; set; }
    
    [Required]
    [Display(Name = "Country")]
    [StringLength(200)]
    public string Country { get; set; }

    [Display(Name = "Address")]
    public string ShortAddress => City + ", " + StreetName + " " + StreetNumber;
    
    [Display(Name = "Full Address")]
    public string FullAddress => StreetName + " " + StreetNumber + "\n" + PostalCode + " " + City + "\n" + Country;
}