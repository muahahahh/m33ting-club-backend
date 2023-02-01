using System.Collections.Generic;
using FluentAssertions;
using M33tingClub.Domain.Meetings;
using M33tingClub.Domain.Meetings.Rules;
using M33tingClub.Domain.Users;
using M33tingClub.UnitTests.Utilities;
using NUnit.Framework;

namespace M33tingClub.UnitTests.Meetings.Rules;

[TestFixture]
public class CannotAskToJoinMeetingTwiceRuleTests : TestBase
{
    [Test]
    public void GivenUserMeetingApplicationNotExists_WhenCheckCannotAskToJoinMeetingTwiceRule_ThenRuleIsNotBroken()
    {
        // Given
        var userId = UserId.CreateNew();
        var applications = new List<MeetingApplication>();

        // When
        var isBroken = new CannotAskToJoinMeetingTwiceRule(userId, applications).IsBroken();

        // Then
        isBroken.Should().BeFalse();
    }
    
    [Test]
    public void GivenUserMeetingApplicationExists_WhenCheckCannotAskToJoinMeetingTwiceRule_ThenRuleIsBroken()
    {
        // Given
        var userId = UserId.CreateNew();
        var applications = new List<MeetingApplication>();
        applications.Add(MeetingApplication.CreatePending(userId));

        // When
        var isBroken = new CannotAskToJoinMeetingTwiceRule(userId, applications).IsBroken();

        // Then
        isBroken.Should().BeTrue();
    }
}