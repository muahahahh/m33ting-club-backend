using M33tingClub.Domain.MeetingNotifications;
using M33tingClub.Domain.Meetings.DomainEvents;
using MediatR;

namespace M33tingClub.Application.MeetingNotifications;

public class ApplicationAcceptedEventHandler : NotificationHandler<ApplicationAcceptedDomainEvent>
{
    private readonly IMeetingNotificationRepository _meetingNotificationRepository;

    public ApplicationAcceptedEventHandler(IMeetingNotificationRepository meetingNotificationRepository)
    {
        _meetingNotificationRepository = meetingNotificationRepository;
    }

    protected override void Handle(ApplicationAcceptedDomainEvent notification)
    {
        var meetingNotification = MeetingNotification.Create(
            MeetingNotificationType.ApplicationAccepted,
            notification.MeetingId,
            notification.UserWhoAcceptedApplicationId,
            notification.AcceptedUserId,
            notification.OccurredOn);

        _meetingNotificationRepository.Add(meetingNotification);
    }
}