using M33tingClub.Application.Utilities;
using M33tingClub.Domain.Auth;
using MediatR;

namespace M33tingClub.Application.MeetingNotifications.MarkMeetingNotificationsAsSeen;

public class MarkMeetingNotificationsAsSeenCommandHandler : ICommandHandler<MarkMeetingNotificationsAsSeenCommand, Unit>
{
    private readonly IMeetingNotificationRepository _meetingNotificationRepository;
    private readonly IUserContext _userContext;

    public MarkMeetingNotificationsAsSeenCommandHandler(
        IMeetingNotificationRepository meetingNotificationRepository, 
        IUserContext userContext)
    {
        _meetingNotificationRepository = meetingNotificationRepository;
        _userContext = userContext;
    }
    
    public async Task<Unit> Handle(MarkMeetingNotificationsAsSeenCommand command, CancellationToken cancellationToken)
    {
        var currentUserId = _userContext.UserId;

        var meetingNotifications = await _meetingNotificationRepository.GetMultiple(command.Ids);

        foreach (var meetingNotification in meetingNotifications)
        {
            meetingNotification.MarkAsSeen(currentUserId);
        }
        
        return Unit.Value;
    }
}