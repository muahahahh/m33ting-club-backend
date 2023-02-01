using M33tingClub.Domain.Utilities;

namespace M33tingClub.Domain.Meetings.DomainEvents;

public class ApplicationAcceptedDomainEvent : DomainEventBase
{
    public Guid AcceptedUserId { get; }
    
    public Guid MeetingId { get; }
    
    public Guid UserWhoAcceptedApplicationId { get; }
    
    public ApplicationAcceptedDomainEvent(
        Guid acceptedUserId, 
        Guid meetingId, 
        Guid userWhoAcceptedApplicationId)
    {
        AcceptedUserId = acceptedUserId;
        MeetingId = meetingId;
        UserWhoAcceptedApplicationId = userWhoAcceptedApplicationId;
    }
}