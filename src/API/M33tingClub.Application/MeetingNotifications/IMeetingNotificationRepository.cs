using M33tingClub.Domain.MeetingNotifications;

namespace M33tingClub.Application.MeetingNotifications;

public interface IMeetingNotificationRepository
{
    Task Add(MeetingNotification meetingNotification);

    Task<List<MeetingNotification>> GetMultiple(List<Guid> ids);
}