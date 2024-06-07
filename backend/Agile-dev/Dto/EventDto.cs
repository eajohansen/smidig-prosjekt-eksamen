using agile_dev.Models;

namespace agile_dev.Dto;

// Dto, Data transfer Object, an in between class for transfer from frontend to backend because of dateTime from frontend to backend. 
public class EventDto {
    public string Title { get; set; }
    public string? Description { get; set; }
    public bool Published { get; set; }
    public Place? Place { get; set; }
    public int? ImageId { get; set; }
    public Image? Image { get; set; }
    public ContactPerson? ContactPerson { get; set; }
    public int OrganizationId { get; set; }
    public string? Start { get; set; }
    public string? StartTime { get; set; }
    public string? End { get; set; }
    public string? EndTime { get; set; }
}