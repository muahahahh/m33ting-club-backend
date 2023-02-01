using M33tingClub.Domain.Utilities;

namespace M33tingClub.Domain.Meetings.Rules;

public class ParticipantsLimitShouldBeNullOrGreaterThanTwoRule : IRule
{
    private readonly int? _participantsLimit;

    public ParticipantsLimitShouldBeNullOrGreaterThanTwoRule(int? participantsLimit)
    {
        _participantsLimit = participantsLimit;
    }
    public bool IsBroken()
    {
        return _participantsLimit is < 2;
    }

    public string Message => "Participants limit should be greater than two or empty";
    public RuleExceptionKind Kind => RuleExceptionKind.BadRequest;
}