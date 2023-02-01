using System.Collections.Generic;
using FluentAssertions;
using M33tingClub.Domain.Meetings;
using M33tingClub.Domain.Meetings.Rules;
using M33tingClub.Domain.Users;
using M33tingClub.Domain.Utilities;
using M33tingClub.UnitTests.Utilities;
using NUnit.Framework;

namespace M33tingClub.UnitTests.Meetings.Rules;

public class OnlyLeaderOrMemberCanLeaveMeetingRuleTests : TestBase
{
    [Test]
    public void GivenMeeting_WhenCheckOnlyLeaderOrMemberCanLeaveMeeting_AndMemberLeavesMeeting_ThenRuleIsNotBroken()
    {
        // Given
        var userId = UserId.CreateNew();
        var participants = new List<Participant>();
        var owner = Participant.Create(userId, MeetingRole.Member, Clock.Now);
        participants.Add(owner);
        
        // When
        var isBroken = new OnlyLeaderOrMemberCanLeaveMeetingRule(userId, participants).IsBroken();

        // Then
        isBroken.Should().BeFalse();
    }
    
    [Test]
    public void GivenMeeting_WhenCheckOnlyLeaderOrMemberCanLeaveMeeting_AndLeaderLeavesMeeting_ThenRuleIsNotBroken()
    {
        // Given
        var userId = UserId.CreateNew();
        var participants = new List<Participant>();
        var leader = Participant.Create(userId, MeetingRole.Leader, Clock.Now);
        participants.Add(leader);
        
        // When
        var isBroken = new OnlyLeaderOrMemberCanLeaveMeetingRule(userId, participants).IsBroken();

        // Then
        isBroken.Should().BeFalse();
    }
    
    [Test]
    public void GivenMeeting_WhenCheckOnlyLeaderOrMemberCanLeaveMeeting_AndOwnerLeavesMeeting_ThenRuleIsNotBroken()
    {
        // Given
        var userId = UserId.CreateNew();
        var participants = new List<Participant>();
        var leader = Participant.Create(userId, MeetingRole.Owner, Clock.Now);
        participants.Add(leader);
        
        // When
        var isBroken = new OnlyLeaderOrMemberCanLeaveMeetingRule(userId, participants).IsBroken();

        // Then
        isBroken.Should().BeTrue();
    }
}