using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace agile_dev.Models;

public class Event {
    
    /*
       Data annotations

       [KEY] = Data annotation for primary key of this model
       [Required] = Data annotation for making it a necessary field for the row
       [Display(Name = "*name*")] = Data annotation for which name is showing in when one looks at the database
       [StringLength(*number*)] = Data annotation for setting a max length on the field
       [ForeignKey("*key*")] = Data annotation for choosing which element to connect up a relationship

    */

    public Event(string title) {
        Title = title;
    }
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Display(Name = "Event Id")]
    public int EventId { get; set; }
    
    [Required]
    [Display(Name = "Title")]
    [StringLength(200)]
    public string Title { get; set; }
    
    [Display(Name = "Description")]
    [StringLength(3000)]
    public string? Description { get; set; }
    
    [Required]
    [Display(Name = "Published")]
    public bool Published { get; set; }
     
    [Display(Name = "Event DateTime id")]
    [ForeignKey("EventDateTimeId")]
    public int? EventDateTimeId { get; set; }
    
    [Display(Name = "Place id")]
    [ForeignKey("PlaceId")]
    public int? PlaceId { get; set; }
    
    [Required]
    [Display(Name = "Image id")]
    [ForeignKey("ImageId")]
    public int ImageId { get; set; }
    
    [Display(Name = "Contact person id")]
    [ForeignKey("ContactPersonId")]
    public int? ContactPersonId  { get; set; }
    
    // A HasSet of all EventCustomFields with this Event
    public ICollection<EventCustomField>? EventCustomFields { get; set; }
    
    // A HasSet of all UserEvents with this Event
    public ICollection<UserEvent>? UserEvents { get; set; }
}