using M33tingClub.Application.Meetings;
using M33tingClub.Application.Utilities;
using M33tingClub.Domain.Auth;
using M33tingClub.Domain.Meetings;
using M33tingClub.Domain.Users;
using MediatR;

namespace M33tingClub.Application.MeetingApplications.RejectMeetingApplication;

public class RejectMeetingApplicationCommandHandler : ICommandHandler<RejectMeetingApplicationCommand, Unit>
{
    private readonly IMeetingRepository _meetingRepository;

    private readonly IUserContext _userContext;
    
    public RejectMeetingApplicationCommandHandler(
        IMeetingRepository meetingRepository, 
        IUserContext userContext)
    {
        _meetingRepository = meetingRepository;
        _userContext = userContext;
    }
    
    public async Task<Unit> Handle(RejectMeetingApplicationCommand command, CancellationToken cancellationToken)
    {
        var currentUserId = UserId.FromGuid(_userContext.UserId);
        var acceptedUserId = UserId.FromGuid(command.UserId);
        var meeting = await _meetingRepository.GetOrThrow(MeetingId.FromGuid(command.MeetingId));
        meeting.RejectApplication(currentUserId, acceptedUserId);

        return await Unit.Task;
    }
}