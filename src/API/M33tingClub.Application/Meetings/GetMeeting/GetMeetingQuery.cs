using M33tingClub.Application.Utilities;

namespace M33tingClub.Application.Meetings.GetMeeting;

public record GetMeetingQuery(Guid MeetingId, 
    double Longitude,
    double Latitude) : IQuery<MeetingDTO?>;