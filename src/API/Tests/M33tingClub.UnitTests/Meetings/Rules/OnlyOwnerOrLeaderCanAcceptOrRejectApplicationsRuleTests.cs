﻿using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using M33tingClub.Domain.Meetings;
using M33tingClub.Domain.Meetings.Rules;
using M33tingClub.Domain.Users;
using M33tingClub.Domain.Utilities;
using M33tingClub.UnitTests.Utilities;
using NUnit.Framework;

namespace M33tingClub.UnitTests.Meetings.Rules;

public class OnlyOwnerOrLeaderCanAcceptOrRejectApplicationsRuleTests : TestBase
{
    [Test]
    public void GivenMeeting_WhenCheckOnlyOwnerOrLeaderCancceptOrRejectApplicationsRule_AndOwnerAcceptsApplication_ThenRuleIsNotBroken()
    {
        // Given
        var userId = UserId.CreateNew();
        var participants = new List<Participant>();
        var owner = Participant.Create(userId, MeetingRole.Owner, Clock.Now);
        participants.Add(owner);
        
        // When
        var isBroken = new OnlyOwnerOrLeaderCanAcceptOrRejectApplicationsRule(userId, participants).IsBroken();

        // Then
        isBroken.Should().BeFalse();
    }
    
    [Test]
    public void GivenMeeting_WhenCheckOnlyOwnerOrLeaderCancceptOrRejectApplicationsRule_AndLeaderAcceptsApplication_ThenRuleIsNotBroken()
    {
        // Given
        var userId = UserId.CreateNew();
        var participants = new List<Participant>();
        var leader = Participant.Create(userId, MeetingRole.Leader, Clock.Now);
        participants.Add(leader);
        
        // When
        var isBroken = new OnlyOwnerOrLeaderCanAcceptOrRejectApplicationsRule(userId, participants).IsBroken();

        // Then
        isBroken.Should().BeFalse();
    }
    
    [Test]
    public void GivenMeeting_WhenCheckOnlyOwnerOrLeaderCanAcceptOrRejectApplicationsRule_AndMemberAcceptsApplication_ThenRuleIsBroken()
    {
        // Given
        var userId = UserId.CreateNew();
        var participants = new List<Participant>();
        var member = Participant.Create(userId, MeetingRole.Member, Clock.Now);
        participants.Add(member);
        
        // When
        var isBroken = new OnlyOwnerOrLeaderCanAcceptOrRejectApplicationsRule(userId, participants).IsBroken();

        // Then
        isBroken.Should().BeTrue();
    }
}