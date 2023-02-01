using M33tingClub.Domain.Utilities;

namespace M33tingClub.Domain.Meetings.DomainEvents;

public class UserAskedToJoinMeetingDomainEvent : DomainEventBase
{
    public Guid AskedToJoidUserId { get; }
    
    public Guid MeetingId { get; }
    
    public Guid MeetingOwnerId { get; }
    
    public UserAskedToJoinMeetingDomainEvent(
        Guid askedToJoidUserId, 
        Guid meetingId, 
        Guid meetingOwnerId)
    {
        AskedToJoidUserId = askedToJoidUserId;
        MeetingId = meetingId;
        MeetingOwnerId = meetingOwnerId;
    }
}