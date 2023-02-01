using M33tingClub.Domain.Meetings.Rules;
using M33tingClub.Domain.Utilities;

namespace M33tingClub.Domain.Meetings;

public class TimeRange : ValueObject
{
	public DateTimeOffset Start { get; }

	public DateTimeOffset End { get; }

	private TimeRange(DateTimeOffset start, DateTimeOffset end)
	{
		CheckRule(new StartDateMustBeLessThanEndDateRule(start, end));

		Start = start;
		End = end;
	}

	public static TimeRange Create(DateTimeOffset start, DateTimeOffset end)
		=> new(start, end);

	public bool IsInRange(DateTimeOffset time)
		=> time >= Start && time <= End;

	public TimeRange Copy()
		=> new(Start, End);
}