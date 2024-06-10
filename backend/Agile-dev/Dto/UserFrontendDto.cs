using agile_dev.Models;

namespace agile_dev.Dto;

public class UserFrontendDto {
    public string? Id { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? Birthdate { get; set; }
    public string? ExtraInfo { get; set; }
    public ICollection<Follower>? FollowOrganization { get; set; }
    public ICollection<Organizer>? OrganizerOrganization { get; set; }
    public ICollection<UserEvent>? UserEvents { get; set; }
    public ICollection<Notice>? Notices { get; set; }
    public ICollection<Allergy>? Allergies { get; set; }
}