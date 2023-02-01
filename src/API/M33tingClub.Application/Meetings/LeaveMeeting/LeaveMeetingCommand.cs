using M33tingClub.Application.Utilities;
using MediatR;

namespace M33tingClub.Application.Meetings.LeaveMeeting;

public record LeaveMeetingCommand(
    Guid MeetingId) : ICommand<Unit>;