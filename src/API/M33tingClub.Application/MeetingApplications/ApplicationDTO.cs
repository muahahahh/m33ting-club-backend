namespace M33tingClub.Application.MeetingApplications;

public class ApplicationDTO
{
    public Guid MeetingId { get; set; }

    public Guid UserId { get; set; }

    public string Status { get; set; }

    public string Name { get; set; }

    public Guid? ImageId { get; set; }
}