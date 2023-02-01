using M33tingClub.Domain.Utilities;

namespace M33tingClub.Domain.Meetings.Rules;

public class CannotJoinMeetingWhenParticipantsLimitIsReachedRule : IRule
{
	private readonly int _numberOfParticipants;

	private readonly int? _participantsLimit;

	public CannotJoinMeetingWhenParticipantsLimitIsReachedRule(
		int numberOfParticipants, 
		int? participantsLimit)
	{
		_numberOfParticipants = numberOfParticipants;
		_participantsLimit = participantsLimit;
	}

	public bool IsBroken()
		=> _participantsLimit.HasValue && _numberOfParticipants >= _participantsLimit;

	public string Message => "Cannot join to meeting. Participants limit reached.";
	
	public RuleExceptionKind Kind => RuleExceptionKind.Conflict;
}