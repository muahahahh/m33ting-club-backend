using M33tingClub.Application.Utilities;

namespace M33tingClub.Application.MeetingNotifications.GetUnseenMeetingNotifications;

public record GetUnseenMeetingNotificationsQuery(List<string>? Types = null) : IQuery<List<MeetingNotificationDTO>>;