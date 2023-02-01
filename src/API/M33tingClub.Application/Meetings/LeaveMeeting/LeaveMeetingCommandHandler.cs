using M33tingClub.Application.Meetings.JoinMeeting;
using M33tingClub.Application.Utilities;
using M33tingClub.Domain.Auth;
using M33tingClub.Domain.Meetings;
using M33tingClub.Domain.Users;
using MediatR;

namespace M33tingClub.Application.Meetings.LeaveMeeting;

public class LeaveMeetingCommandHandler : ICommandHandler<LeaveMeetingCommand, Unit>
{
    private readonly IMeetingRepository _meetingRepository;

    private readonly IUserContext _userContext;
    
    public LeaveMeetingCommandHandler(
        IMeetingRepository meetingRepository, 
        IUserContext userContext)
    {
        _meetingRepository = meetingRepository;
        _userContext = userContext;
    }

    public async Task<Unit> Handle(LeaveMeetingCommand command, CancellationToken cancellationToken)
    {
        var meeting = await _meetingRepository.GetOrThrow(MeetingId.FromGuid(command.MeetingId));
        var userId = UserId.FromGuid(_userContext.UserId);
		
        meeting.LeaveMeeting(userId);
		
        return await Unit.Task;
    }
}