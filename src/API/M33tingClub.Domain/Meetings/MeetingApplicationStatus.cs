using M33tingClub.Domain.Utilities;

namespace M33tingClub.Domain.Meetings;

public class MeetingApplicationStatus : ValueObject
{
    public string Name { get; }

    public static MeetingApplicationStatus Pending => new(nameof(Pending));
    
    public static MeetingApplicationStatus Accepted => new(nameof(Accepted));

    public static MeetingApplicationStatus Rejected => new(nameof(Rejected));

    private MeetingApplicationStatus(string name)
    {
        Name = name;
    }

    public static MeetingApplicationStatus FromName(string name)
        => new(name);
}