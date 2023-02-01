namespace M33tingClub.Application.Meetings.GetFeed;

public class FeedDTO
{
    public Guid UserId { get; set; }
    
    public string UserName { get; set; }
    
    public Guid? UserImageId { get; set; }
    
    public string MeetingRole { get; set; }
    
    public Guid MeetingId { get; set; }
    
    public string MeetingName { get; set; }
    
    public Guid MeetingImageId { get; set; }
    
    public DateTimeOffset OccuredOn { get; set; }
}