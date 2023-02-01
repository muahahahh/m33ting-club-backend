using M33tingClub.Application.Meetings;
using M33tingClub.Application.Utilities;
using M33tingClub.Domain.Auth;
using M33tingClub.Domain.Meetings;
using M33tingClub.Domain.Users;
using M33tingClub.Domain.Utilities;
using MediatR;

namespace M33tingClub.Application.MeetingApplications.AcceptMeetingApplication;

public class AcceptMeetingApplicationCommandHandler : ICommandHandler<AcceptMeetingApplicationCommand, Unit>
{
    private readonly IMeetingRepository _meetingRepository;

    private readonly IUserContext _userContext;
    
    public AcceptMeetingApplicationCommandHandler(
        IMeetingRepository meetingRepository, 
        IUserContext userContext)
    {
        _meetingRepository = meetingRepository;
        _userContext = userContext;
    }
    
    public async Task<Unit> Handle(AcceptMeetingApplicationCommand command, CancellationToken cancellationToken)
    {
        var currentUserId = UserId.FromGuid(_userContext.UserId);
        var acceptedUserId = UserId.FromGuid(command.UserId);
        var meeting = await _meetingRepository.GetOrThrow(MeetingId.FromGuid(command.MeetingId));
        meeting.AcceptApplication(currentUserId, acceptedUserId, Clock.Now);

        return await Unit.Task;
    }
}