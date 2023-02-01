using M33tingClub.Domain.Utilities;

namespace M33tingClub.Domain.Meetings;

public class MeetingStatus : ValueObject
{
    public string Name { get; }

    public static MeetingStatus Cancelled => new(nameof(Cancelled));

    public static MeetingStatus Finished => new(nameof(Finished));

    public static MeetingStatus Ongoing => new(nameof(Ongoing));

    public static MeetingStatus Upcoming => new(nameof(Upcoming));
    
    public static MeetingStatus FromName(string name)
        => new(name);

    private MeetingStatus(string name)
    {
        Name = name;
    }

    public MeetingStatus Copy()
        => new(Name);
}