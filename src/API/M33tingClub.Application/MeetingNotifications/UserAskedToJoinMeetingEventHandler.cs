using M33tingClub.Domain.MeetingNotifications;
using M33tingClub.Domain.Meetings.DomainEvents;
using MediatR;

namespace M33tingClub.Application.MeetingNotifications;

public class UserAskedToJoinMeetingEventHandler : NotificationHandler<UserAskedToJoinMeetingDomainEvent>
{
    private readonly IMeetingNotificationRepository _meetingNotificationRepository;

    public UserAskedToJoinMeetingEventHandler(IMeetingNotificationRepository meetingNotificationRepository)
    {
        _meetingNotificationRepository = meetingNotificationRepository;
    }

    protected override void Handle(UserAskedToJoinMeetingDomainEvent notification)
    {
        var meetingNotification = MeetingNotification.Create(
            MeetingNotificationType.UserAskedToJoin,
            notification.MeetingId,
            notification.AskedToJoidUserId,
            notification.MeetingOwnerId,
            notification.OccurredOn);

        _meetingNotificationRepository.Add(meetingNotification);
    }
}