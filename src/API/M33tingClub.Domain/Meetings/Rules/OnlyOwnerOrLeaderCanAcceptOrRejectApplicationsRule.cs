using M33tingClub.Domain.Users;
using M33tingClub.Domain.Utilities;

namespace M33tingClub.Domain.Meetings.Rules;

public class OnlyOwnerOrLeaderCanAcceptOrRejectApplicationsRule : IRule
{
    private readonly UserId _userId;

    private readonly List<Participant> _participants;

    public OnlyOwnerOrLeaderCanAcceptOrRejectApplicationsRule(UserId userId, List<Participant> participants)
    {
        _userId = userId;
        _participants = participants;
    }

    public bool IsBroken()
    {
        var meetingRole = _participants.SingleOrDefault(x => x.UserId == _userId)?.MeetingRole;
        
        return meetingRole is null || !meetingRole.IsOwnerOrLeader();
    }

    public string Message => "Only Owner or Leader can accept or reject applications.";
    
    public RuleExceptionKind Kind => RuleExceptionKind.BadRequest;
}