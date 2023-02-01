using M33tingClub.Domain.Users;
using M33tingClub.Domain.Utilities;

namespace M33tingClub.Domain.Meetings.Rules;

public class OnlyLeaderOrMemberCanLeaveMeetingRule : IRule
{
    private readonly UserId _userId;

    private readonly List<Participant> _participants;

    public OnlyLeaderOrMemberCanLeaveMeetingRule(UserId userId, List<Participant> participants)
    {
        _userId = userId;
        _participants = participants;
    }

    public bool IsBroken()
    {
        var meetingRole = _participants.SingleOrDefault(x => x.UserId == _userId)?.MeetingRole;
        
        return meetingRole is null || !meetingRole.IsLeaderOrMember();
    }

    public string Message => "Only Leader or Member can leave the meeting.";
    
    public RuleExceptionKind Kind => RuleExceptionKind.BadRequest;
}