using M33tingClub.Application.Utilities;
using MediatR;

namespace M33tingClub.Application.Meetings.JoinMeeting;

public record JoinMeetingCommand(
	Guid MeetingId) : ICommand<Unit>;