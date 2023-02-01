using M33tingClub.Domain.MeetingNotifications.Rules;
using M33tingClub.Domain.Utilities;

namespace M33tingClub.Domain.MeetingNotifications;

public class MeetingNotification : Entity
{
    public Guid Id { get; }

    private MeetingNotificationType _type;

    private Guid _meetingId;
    
    private Guid _performerId;

    private Guid _receiverId;

    private bool _wasSeen;

    private DateTimeOffset _occuredOn;

    private MeetingNotification()
    {
        // For EF core
    }

    private MeetingNotification(
        MeetingNotificationType type, 
        Guid meetingId, 
        Guid performerId, 
        Guid receiverId,
        DateTimeOffset occuredOn)
    {
        Id = Guid.NewGuid();
        _type = type;
        _meetingId = meetingId;
        _performerId = performerId;
        _receiverId = receiverId;
        _occuredOn = occuredOn;
        _wasSeen = false;
    }

    public static MeetingNotification Create(
        MeetingNotificationType type, 
        Guid meetingId, 
        Guid performerId, 
        Guid receiverId,
        DateTimeOffset occuredOn)
    {
        return new(
            type,
            meetingId,
            performerId,
            receiverId,
            occuredOn);
    }

    public void MarkAsSeen(Guid currentUserId)
    {
        CheckRule(new OnlyReceiverCanMarkMeetingNotificationAsSeenRule(_receiverId, currentUserId));
        
        _wasSeen = true;
    }
}