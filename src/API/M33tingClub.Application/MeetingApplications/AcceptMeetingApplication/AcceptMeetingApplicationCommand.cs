using M33tingClub.Application.Utilities;
using MediatR;

namespace M33tingClub.Application.MeetingApplications.AcceptMeetingApplication;

public record AcceptMeetingApplicationCommand(Guid MeetingId, Guid UserId) : ICommand<Unit>;

