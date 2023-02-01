using M33tingClub.Application.Utilities;
using M33tingClub.Domain.Auth;
using M33tingClub.Domain.Meetings;
using M33tingClub.Domain.Users;
using MediatR;

namespace M33tingClub.Application.Meetings.CancelMeeting;

public class CancelMeetingCommandHandler : ICommandHandler<CancelMeetingCommand, Unit>
{
    private readonly IMeetingRepository _meetingRepository;

    private readonly IUserContext _userContext;

    public CancelMeetingCommandHandler(
        IMeetingRepository meetingRepository,
        IUserContext userContext)
    {
        _meetingRepository = meetingRepository;
        _userContext = userContext;
    }

    public async Task<Unit> Handle(CancelMeetingCommand command, CancellationToken cancellationToken)
    {
        var meeting = await _meetingRepository.GetOrThrow(MeetingId.FromGuid(command.Id));
        var userId = UserId.FromGuid(_userContext.UserId);

        meeting.TryCancel(userId);
        
        return await Unit.Task;
    }
}