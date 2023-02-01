using M33tingClub.Application.Utilities;

namespace M33tingClub.Application.Meetings.GetMeetings;

public record GetMeetingsForExploreQuery(
	List<string> Tags,
	double Longitude,
    double Latitude,
	int Limit,
	int Offset) : IQuery<PagingInfo<MeetingDTO>>;