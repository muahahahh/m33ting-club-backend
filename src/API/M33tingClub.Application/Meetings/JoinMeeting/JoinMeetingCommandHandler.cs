using M33tingClub.Application.Utilities;
using M33tingClub.Domain.Auth;
using M33tingClub.Domain.Meetings;
using M33tingClub.Domain.Users;
using M33tingClub.Domain.Utilities;
using MediatR;

namespace M33tingClub.Application.Meetings.JoinMeeting;

public class JoinMeetingCommandHandler : ICommandHandler<JoinMeetingCommand, Unit>
{
	private readonly IMeetingRepository _meetingRepository;

	private readonly IUserContext _userContext;

	public JoinMeetingCommandHandler(
		IMeetingRepository meetingRepository, 
		IUserContext userContext)
	{
		_meetingRepository = meetingRepository;
		_userContext = userContext;
	}

	public async Task<Unit> Handle(JoinMeetingCommand command, CancellationToken cancellationToken)
	{
		var meeting = await _meetingRepository.GetOrThrow(MeetingId.FromGuid(command.MeetingId));
		var userId = UserId.FromGuid(_userContext.UserId);
		
		meeting.JoinOrAskToJoin(userId, Clock.Now);
		
		return await Unit.Task;
	}
}