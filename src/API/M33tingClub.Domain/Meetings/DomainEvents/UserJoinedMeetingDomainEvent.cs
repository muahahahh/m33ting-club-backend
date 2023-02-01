using M33tingClub.Domain.Utilities;

namespace M33tingClub.Domain.Meetings.DomainEvents;

public class UserJoinedMeetingDomainEvent : DomainEventBase
{
    public Guid JoinedUserId { get; }
    
    public Guid MeetingId { get; }
    
    public Guid MeetingOwnerId { get; }
    
    public UserJoinedMeetingDomainEvent(
        Guid joinedUserId, 
        Guid meetingId, 
        Guid meetingOwnerId)
    {
        JoinedUserId = joinedUserId;
        MeetingId = meetingId;
        MeetingOwnerId = meetingOwnerId;
    }
}