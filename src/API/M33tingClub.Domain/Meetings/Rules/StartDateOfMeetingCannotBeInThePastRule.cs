using M33tingClub.Domain.Utilities;

namespace M33tingClub.Domain.Meetings.Rules;

public class StartDateOfMeetingCannotBeInThePastRule : IRule
{
	private readonly DateTimeOffset _startDate;
	private readonly DateTimeOffset _currentDate;

	public StartDateOfMeetingCannotBeInThePastRule(DateTimeOffset startDate, DateTimeOffset currentDate)
	{
		_startDate = startDate;
		_currentDate = currentDate;
	}

	public bool IsBroken()
		=> _startDate < _currentDate;

	public string Message => "Start date of meeting cannot be in the past";
	
	public RuleExceptionKind Kind => RuleExceptionKind.BadRequest;
}