using M33tingClub.Domain.Users;
using M33tingClub.Domain.Utilities;

namespace M33tingClub.Domain.Meetings.Rules;

public class CannotAskToJoinMeetingTwiceRule : IRule
{
    private readonly UserId _userId;

    private readonly List<MeetingApplication> _applications;

    public CannotAskToJoinMeetingTwiceRule(UserId userId, List<MeetingApplication> applications)
    {
        _userId = userId;
        _applications = applications;
    }

    public bool IsBroken()
        => _applications.Exists(x => x.UserId == _userId);

    public string Message => "Cannot ask to join meeting twice.";
    
    public RuleExceptionKind Kind => RuleExceptionKind.Conflict;
}

//TODO: Add tests