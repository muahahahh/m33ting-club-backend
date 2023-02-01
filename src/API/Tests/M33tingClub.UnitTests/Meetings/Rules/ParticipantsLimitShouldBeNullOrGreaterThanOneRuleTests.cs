using FluentAssertions;
using M33tingClub.Domain.Meetings.Rules;
using M33tingClub.UnitTests.Utilities;
using NUnit.Framework;

namespace M33tingClub.UnitTests.Meetings.Rules;

public class ParticipantsLimitShouldBeNullOrGreaterThanOneRuleTests : TestBase
{
    [Test]
    [TestCase(0, ExpectedResult = true)]
    [TestCase(1, ExpectedResult = true)]
    [TestCase(2, ExpectedResult = false)]
    [TestCase(3, ExpectedResult = false)]
    [TestCase(11, ExpectedResult = false)]
    [TestCase(null, ExpectedResult = false)]
    public bool GivenNumberOfParticipants_WhenCheckedNumberOfParticipantsIsBetweenOneAndTen_ThenExpectedValueIsReturned(
        int? numberOfParticipants)
        => new ParticipantsLimitShouldBeNullOrGreaterThanTwoRule(numberOfParticipants).IsBroken();
}