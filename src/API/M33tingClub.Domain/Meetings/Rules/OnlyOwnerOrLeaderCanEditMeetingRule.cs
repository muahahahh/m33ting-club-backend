using M33tingClub.Domain.Users;
using M33tingClub.Domain.Utilities;

namespace M33tingClub.Domain.Meetings.Rules;

public class OnlyOwnerOrLeaderCanEditMeetingRule : IRule
{
    private readonly UserId _userId;

    private readonly List<Participant> _participants;

    public OnlyOwnerOrLeaderCanEditMeetingRule(UserId userId, List<Participant> participants)
    {
        _userId = userId;
        _participants = participants;
    }

    public bool IsBroken()
    {
        var meetingRole = _participants.SingleOrDefault(x => x.UserId == _userId)?.MeetingRole;
        
        return meetingRole is null || !meetingRole.IsOwnerOrLeader();
    }

    public string Message => "Only Owner or Leader can edit meeting.";
    
    public RuleExceptionKind Kind => RuleExceptionKind.BadRequest;
}