using M33tingClub.Domain.Utilities;

namespace M33tingClub.Domain.MeetingNotifications;

public class MeetingNotificationType : ValueObject
{
    public string Name { get; }

    public static MeetingNotificationType UserJoined => new(nameof(UserJoined));
    
    public static MeetingNotificationType UserAskedToJoin => new(nameof(UserAskedToJoin));
    
    public static MeetingNotificationType ApplicationAccepted => new(nameof(ApplicationAccepted));

    private MeetingNotificationType(string name)
    {
        Name = name;
    }

    public static MeetingNotificationType FromName(string name)
        => new(name);
}