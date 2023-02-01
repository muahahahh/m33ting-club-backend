using M33tingClub.Domain.Utilities;

namespace M33tingClub.Domain.Meetings.Rules;

public class StartDateMustBeLessThanEndDateRule : IRule
{
	private readonly DateTimeOffset _startDate;

	private readonly DateTimeOffset _endDate;

	public StartDateMustBeLessThanEndDateRule(DateTimeOffset startDate, DateTimeOffset endDate)
	{
		_startDate = startDate;
		_endDate = endDate;
	}

	public bool IsBroken()
		=> _startDate >= _endDate;

	public string Message => "Start date must be less than end date";
	
	public RuleExceptionKind Kind => RuleExceptionKind.BadRequest;
}