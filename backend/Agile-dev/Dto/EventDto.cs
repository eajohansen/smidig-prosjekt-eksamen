using agile_dev.Models;

namespace agile_dev.Dto;

// Dto, Data transfer Object, an in between class for transfer from frontend to backend because of dateTime from frontend to backend. 
public class EventDto {
    public Event Event { get; set; } = new();
    public string? Start { get; set; }
    public string? StartTime { get; set; }
    public string? End { get; set; }
    public string? EndTime { get; set; }
}