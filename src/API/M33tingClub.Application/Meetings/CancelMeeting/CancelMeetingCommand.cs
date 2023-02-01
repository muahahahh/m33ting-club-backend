using M33tingClub.Application.Utilities;
using MediatR;

namespace M33tingClub.Application.Meetings.CancelMeeting;

public record CancelMeetingCommand(Guid Id) : ICommand<Unit>;