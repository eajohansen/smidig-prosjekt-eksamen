using agile_dev.Models;

namespace agile_dev.Dto;

public class EventDtoBackend {
    public int EventId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Capacity { get; set; }
    public int AgeLimit { get; set; }
    public bool Private { get; set; }
    public bool Published { get; set; }
    public string PlaceLocation { get; set; }
    public string? PlaceUrl { get; set; }
    public string ImageLink { get; set; }
    public string? ImageDescription { get; set; }
    public string ContactPersonName { get; set; }
    public string? ContactPersonEmail { get; set; }
    public string? ContactPersonNumber { get; set; }
    public string OrganizationName { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public ICollection<EventCustomField>? EventCustomFields { get; set; }
}