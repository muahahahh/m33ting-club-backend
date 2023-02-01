using M33tingClub.Application.Utilities;
using MediatR;

namespace M33tingClub.Application.Meetings.UpdateMeetingsStatuses;

public record UpdateMeetingsStatusesCommand : ICommand<Unit>;