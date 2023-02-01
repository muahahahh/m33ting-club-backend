using M33tingClub.Domain.Users;
using M33tingClub.Domain.Utilities;

namespace M33tingClub.Domain.Meetings.Rules;

public class CannotJoinToMeetingTwiceRule : IRule
{
	private readonly UserId _userId;

	private readonly List<Participant> _participants;

	public CannotJoinToMeetingTwiceRule(UserId userId, List<Participant> participants)
	{
		_userId = userId;
		_participants = participants;
	}

	public bool IsBroken()
		=> _participants.Exists(x => x.UserId == _userId);

	public string Message => "Cannot join to meeting twice.";
	
	public RuleExceptionKind Kind => RuleExceptionKind.Conflict;
}