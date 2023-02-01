using M33tingClub.Domain.Meetings.Rules;
using NUnit.Framework;

namespace M33tingClub.UnitTests.Meetings.Rules;

public class CannotJoinMeetingWhenParticipantsLimitIsReachedRuleTests
{
	[Test]
	[TestCase(10, 10, ExpectedResult = true)]
	[TestCase(11, 10, ExpectedResult = true)]
	[TestCase(9, 10, ExpectedResult = false)]
	[TestCase(10, null, ExpectedResult = false)]
	public bool CheckCannotJoinMeetingWhenParticipantsLimitIsReachedRuleIsBroken(
		int numberOfParticipants,
		int? participantsLimit)
		=> new CannotJoinMeetingWhenParticipantsLimitIsReachedRule(numberOfParticipants, participantsLimit).IsBroken();
}