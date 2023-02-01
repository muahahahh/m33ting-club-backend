using M33tingClub.Application.Utilities;

namespace M33tingClub.Application.Meetings.GetOwnMeetings;

public record GetOwnMeetingsQuery(
    List<string> Statuses, 
    string? Role,
    int Limit,
    int Offset) : IQuery<List<MeetingDTO>>;
    
