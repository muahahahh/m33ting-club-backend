using M33tingClub.Domain.Utilities;

namespace M33tingClub.Domain.Meetings.Rules;

public class NumberOfParticipantsShouldNotExceedParticipantsLimitRule : IRule
{
    private readonly int _numberOfParticipants;
    
    private readonly int? _participantsLimit;

    public NumberOfParticipantsShouldNotExceedParticipantsLimitRule(int numberOfParticipants, int? participantsLimit)
    {
        _numberOfParticipants = numberOfParticipants;
        _participantsLimit = participantsLimit;
    }
    public bool IsBroken()
    {
        return _numberOfParticipants > _participantsLimit && _participantsLimit is not null;
    }

    public string Message => "Participants limit is exceeded";
    public RuleExceptionKind Kind => RuleExceptionKind.BadRequest;
}