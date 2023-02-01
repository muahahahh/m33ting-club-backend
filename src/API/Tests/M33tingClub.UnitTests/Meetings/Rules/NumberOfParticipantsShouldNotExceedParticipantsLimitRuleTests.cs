using FluentAssertions;
using M33tingClub.Domain.Meetings.Rules;
using M33tingClub.UnitTests.Utilities;
using NUnit.Framework;

namespace M33tingClub.UnitTests.Meetings.Rules;

public class NumberOfParticipantsShouldNotExceedParticipantsLimitRuleTests : TestBase
{
    [Test]
    [TestCase(1, 5, ExpectedResult = false)]
    [TestCase(5, 5, ExpectedResult = false)]
    [TestCase(5, null, ExpectedResult = false)]
    [TestCase(6, 5, ExpectedResult = true)]
    [TestCase(10, 5, ExpectedResult = true)]
    public bool GivenNumberOfParticipantsAndParticipantsLimit_WhenCheckIfNumberExceedsLimit_ThenExpectedValueIsReturned(
        int numberOfParticipants, 
        int? participantsLimit)
        => new NumberOfParticipantsShouldNotExceedParticipantsLimitRule(numberOfParticipants, participantsLimit).IsBroken();
}