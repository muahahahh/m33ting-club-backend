namespace M33tingClub.Application.MeetingNotifications.GetUnseenMeetingNotifications;

public class MeetingNotificationDTO
{
    public Guid Id { get; set; }

    public string Type { get; set; } = string.Empty;
    
    public Guid MeetingId { get; set; }

    public DateTimeOffset OccuredOn { get; set; }
    
    public Guid PerformerId { get; set; }
    
    public string PerformerName { get; set; } = string.Empty;
}