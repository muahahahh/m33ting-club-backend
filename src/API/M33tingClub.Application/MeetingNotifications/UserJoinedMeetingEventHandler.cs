using M33tingClub.Domain.MeetingNotifications;
using M33tingClub.Domain.Meetings.DomainEvents;
using MediatR;

namespace M33tingClub.Application.MeetingNotifications;

public class UserJoinedMeetingEventHandler : NotificationHandler<UserJoinedMeetingDomainEvent>
{
    private readonly IMeetingNotificationRepository _meetingNotificationRepository;

    public UserJoinedMeetingEventHandler(IMeetingNotificationRepository meetingNotificationRepository)
    {
        _meetingNotificationRepository = meetingNotificationRepository;
    }

    protected override void Handle(UserJoinedMeetingDomainEvent notification)
    {
        var meetingNotification = MeetingNotification.Create(
            MeetingNotificationType.UserJoined,
            notification.MeetingId,
            notification.JoinedUserId,
            notification.MeetingOwnerId,
            notification.OccurredOn);

        _meetingNotificationRepository.Add(meetingNotification);
    }
}