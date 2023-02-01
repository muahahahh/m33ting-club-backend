using M33tingClub.Application.MeetingApplications;
using M33tingClub.Application.Utilities;

namespace M33tingClub.Application.MeetingApplications.GetMeetingApplications;

public record GetMeetingApplicationsQuery(Guid MeetingId) : IQuery<List<ApplicationDTO>>;
