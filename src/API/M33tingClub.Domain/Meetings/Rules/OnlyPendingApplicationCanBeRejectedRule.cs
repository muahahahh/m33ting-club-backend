using M33tingClub.Domain.Utilities;

namespace M33tingClub.Domain.Meetings.Rules;

public class OnlyPendingApplicationCanBeRejectedRule : IRule
{
    private readonly MeetingApplicationStatus _status;

    public OnlyPendingApplicationCanBeRejectedRule(MeetingApplicationStatus status)
    {
        _status = status;
    }

    public bool IsBroken()
        => _status != MeetingApplicationStatus.Pending;

    public string Message => "Only pending application can be rejected";
    
    public RuleExceptionKind Kind => RuleExceptionKind.Conflict;
}