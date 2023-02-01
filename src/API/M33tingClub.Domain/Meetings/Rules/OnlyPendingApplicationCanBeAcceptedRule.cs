using M33tingClub.Domain.Utilities;

namespace M33tingClub.Domain.Meetings.Rules;

public class OnlyPendingApplicationCanBeAcceptedRule : IRule
{
    private readonly MeetingApplicationStatus _status;

    public OnlyPendingApplicationCanBeAcceptedRule(MeetingApplicationStatus status)
    {
        _status = status;
    }

    public bool IsBroken()
        => _status != MeetingApplicationStatus.Pending;

    public string Message => "Only pending application can be accepted";
    
    public RuleExceptionKind Kind => RuleExceptionKind.Conflict;
}