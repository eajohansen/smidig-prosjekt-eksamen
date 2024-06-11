using agile_dev.Models;

namespace agile_dev.Dto;

// Dto to limit the amount of information backend sends when fetching user
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

public class FollowerOrganizationDto {
    public int OrganizationId { get; set; }
}

public class OrganizerOrganizationDto {
    public int OrganizationId { get; set; }
}

public class UserEventsDto {
    public int EventId { get; set; }
    public int? QueueNumber { get; set; }
    public int? Used { get; set; }
}

public class NoticesDto {
    public int NoticeId { get; set; }
    public DateTime Expire { get; set; }
}

public class AllergiesDto {
    public int AllergyId { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
}
