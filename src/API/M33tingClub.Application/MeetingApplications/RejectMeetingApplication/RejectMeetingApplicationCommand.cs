using M33tingClub.Application.Utilities;
using MediatR;

namespace M33tingClub.Application.MeetingApplications.RejectMeetingApplication;

public record RejectMeetingApplicationCommand(Guid MeetingId, Guid UserId) : ICommand<Unit>;

