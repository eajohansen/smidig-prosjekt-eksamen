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
       [DatabaseGenerated(DatabaseGeneratedOption.Identity)] = This specific data annotation gives this model a private counter for id

    */

    public Event() {
        // Initializing them to avoid NULL reference
        EventCustomFields = new HashSet<EventCustomField>();
        UserEvents = new HashSet<UserEvent>();
    }
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Display(Name = "Event Id")]
    public int EventId { get; set; }
    
    [Required]
    [Display(Name = "Title")]
    [StringLength(200)]
    public string Title { get; set; }
    
    [Required]
    [Display(Name = "Description")]
    [StringLength(3000)]
    public string Description { get; set; }
    
    [Required]
    [Display(Name = "Published")]
    public bool Published { get; set; }
    
    [Required]
    [Display(Name = "Capacity")]
    public int Capacity { get; set; }
    
    [Required]
    [Display(Name = "Event DateTime id")]
    public int EventDateTimeId { get; set; }
    
    [ForeignKey("EventDateTimeId")]
    public virtual EventDateTime EventDateTime { get; set; }
    
    [Required]
    [Display(Name = "Place id")]
    public int PlaceId { get; set; }
    
    [ForeignKey("PlaceId")]
    public virtual Place Place { get; set; }
    
    [Required]
    [Display(Name = "Image id")]
    public int ImageId { get; set; }
    
    [ForeignKey("ImageId")]
    public virtual Image Image { get; set; }
    
    [Required]
    [Display(Name = "Contact person id")]
    public int ContactPersonId  { get; set; }
    
    [ForeignKey("ContactPersonId")]
    public virtual ContactPerson? ContactPerson { get; set; }
    
    // A HasSet of all EventCustomFields with this Event
    public virtual ICollection<EventCustomField> EventCustomFields { get; set; }
    
    // A HasSet of all UserEvents with this Event
    public virtual ICollection<UserEvent> UserEvents { get; set; }
}