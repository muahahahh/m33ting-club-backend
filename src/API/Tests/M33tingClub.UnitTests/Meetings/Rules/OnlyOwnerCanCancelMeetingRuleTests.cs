using System.Collections.Generic;
using FluentAssertions;
using M33tingClub.Domain.Meetings;
using M33tingClub.Domain.Meetings.Rules;
using M33tingClub.Domain.Users;
using M33tingClub.Domain.Utilities;
using M33tingClub.UnitTests.Utilities;
using NUnit.Framework;

namespace M33tingClub.UnitTests.Meetings.Rules;

[TestFixture]
public class OnlyOwnerCanCancelMeetingStatusRuleTests : TestBase
{
    [Test]
    public void GivenMeeting_WhenCheckOnlyOwnerCanCancelMeetingRule_AndOwnerCancelsMeeting_ThenRuleIsNotBroken()
    {
        // Given
        var userId = UserId.CreateNew();
        var participants = new List<Participant>();
        var owner = Participant.Create(userId, MeetingRole.Owner, Clock.Now);
        participants.Add(owner);
        
        // When
        var isBroken = new OnlyOwnerCanCancelMeetingRule(userId, participants).IsBroken();
        
        // Then
        isBroken.Should().BeFalse();
    }
    
    [Test]
    public void GivenMeeting_WhenCheckOnlyOwnerCanCancelMeetingRule_AndLeaderCancelsMeeting_ThenRuleIsBroken()
    {
        // Given
        var userId = UserId.CreateNew();
        var participants = new List<Participant>();
        var leader = Participant.Create(userId, MeetingRole.Leader, Clock.Now);
        participants.Add(leader);
        
        // When
        var isBroken = new OnlyOwnerCanCancelMeetingRule(userId, participants).IsBroken();
        
        // Then
        isBroken.Should().BeTrue();
    }
    
    [Test]
    public void GivenMeeting_WhenCheckOnlyOwnerCanCancelMeetingRule_AndMemberCancelsMeeting_ThenRuleIsBroken()
    {
        // Given
        var userId = UserId.CreateNew();
        var participants = new List<Participant>();
        var member = Participant.Create(userId, MeetingRole.Member, Clock.Now);
        participants.Add(member);
        
        // When
        var isBroken = new OnlyOwnerCanCancelMeetingRule(userId, participants).IsBroken();
        
        // Then
        isBroken.Should().BeTrue();
    }
    
    [Test]
    public void GivenMeeting_WhenCheckOnlyOwnerCanCancelMeetingRule_AndNotParticipantCancelsMeeting_ThenRuleIsBroken()
    {
        // Given
        var userId = UserId.CreateNew();
        var participants = new List<Participant>();
        
        // When
        var isBroken = new OnlyOwnerCanCancelMeetingRule(userId, participants).IsBroken();
        
        // Then
        isBroken.Should().BeTrue();
    }
    
}