using M33tingClub.Application.Utilities;
using MediatR;

namespace M33tingClub.Application.MeetingNotifications.MarkMeetingNotificationsAsSeen;

public record MarkMeetingNotificationsAsSeenCommand(List<Guid> Ids) : ICommand<Unit>;