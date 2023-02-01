using System;
using FluentAssertions;
using M33tingClub.Domain.Meetings;
using M33tingClub.Domain.Meetings.Rules;
using M33tingClub.Domain.Users;
using M33tingClub.Domain.Utilities;
using M33tingClub.UnitTests.Utilities;
using NUnit.Framework;

namespace M33tingClub.UnitTests.Meetings.Meetings;

[TestFixture]
public class JoinMeetingTests
{
	[Test]
	public void GivenMeeting_WithoutParticipantLimit_WhenJoinMeeting_ThenUserJoined()
	{
		// Given
		var currentDate = new DateTimeOffset(2022, 6, 21, 18, 0, 0, DateTimeOffset.Now.Offset);
		Clock.Set(currentDate);

		var ownerId = UserId.FromGuid(Guid.Parse("7BC72146-EF6F-46FA-8CAD-074D329B166D"));
		var meeting = new MeetingBuilder()
		              .WithOwner(ownerId)
		              .WithoutParticipantsLimit()
		              .Build();
		
		// When
		var userId = UserId.FromGuid(Guid.Parse("B60B8AD9-1179-4491-AFB0-A5CC83C16D3F"));
		meeting.Join(userId, Clock.Now);

		// Then
		var meetingSnapshot = meeting.GetSnapshot();
		meetingSnapshot.Participants.Should()
           .BeEquivalentTo(
               new[]
               {
	               Participant.Create(ownerId, MeetingRole.Owner, Clock.Now),
	               Participant.Create(userId, MeetingRole.Member, Clock.Now) 
               });
	}
	
	[Test]
	public void GivenMeeting_WithoutParticipantLimitNotReached_WhenJoinMeeting_ThenUserJoined()
	{
		// Given
		var currentDate = new DateTimeOffset(2022, 6, 21, 18, 0, 0, DateTimeOffset.Now.Offset);
		Clock.Set(currentDate);

		var ownerId = UserId.FromGuid(Guid.Parse("7BC72146-EF6F-46FA-8CAD-074D329B166D"));
		var meeting = new MeetingBuilder()
		              .WithOwner(ownerId)
		              .WithParticipantsLimit(2)
		              .Build();
		
		// When
		var userId = UserId.FromGuid(Guid.Parse("B60B8AD9-1179-4491-AFB0-A5CC83C16D3F"));
		meeting.Join(userId, Clock.Now);

		// Then
		var meetingSnapshot = meeting.GetSnapshot();
		meetingSnapshot.Participants.Should()
		               .BeEquivalentTo(
			               new[]
			               {
				               Participant.Create(ownerId, MeetingRole.Owner, Clock.Now),
				               Participant.Create(userId, MeetingRole.Member, Clock.Now) 
			               });
	}
	
	[Test]
	public void GivenMeeting_AndParticipantsLimitReached_WhenJoinMeeting_ThenUserNotJoined()
	{
		// Given
		var currentDate = new DateTimeOffset(2022, 6, 21, 18, 0, 0, DateTimeOffset.Now.Offset);
		Clock.Set(currentDate);

		var ownerId = UserId.FromGuid(Guid.Parse("7BC72146-EF6F-46FA-8CAD-074D329B166D"));
		var meeting = new MeetingBuilder()
		              .WithOwner(ownerId)
		              .WithParticipantsLimit(2)
		              .Build();
		
		// When
		var firstUserId = UserId.FromGuid(Guid.Parse("94B55FD9-5E3D-4C3C-A0B3-97FC6FB93370"));
		meeting.Join(firstUserId, Clock.Now);
		
		var secondUserId = UserId.FromGuid(Guid.Parse("B60B8AD9-1179-4491-AFB0-A5CC83C16D3F"));
		var func = () => meeting.Join(secondUserId, Clock.Now);

		// Then
		func.ShouldBrokeRule<CannotJoinMeetingWhenParticipantsLimitIsReachedRule>();
	}
	
	[Test]
	public void GivenMeeting_AndUserAlreadyJoined_WhenJoinMeeting_ThenUserNotJoined()
	{
		// Given
		var currentDate = new DateTimeOffset(2022, 6, 21, 18, 0, 0, DateTimeOffset.Now.Offset);
		Clock.Set(currentDate);

		var ownerId = UserId.FromGuid(Guid.Parse("7BC72146-EF6F-46FA-8CAD-074D329B166D"));
		var meeting = new MeetingBuilder()
		              .WithOwner(ownerId)
		              .WithoutParticipantsLimit()
		              .Build();
		
		// When
		var userId = UserId.FromGuid(Guid.Parse("B60B8AD9-1179-4491-AFB0-A5CC83C16D3F"));
		meeting.Join(userId, Clock.Now);
		var func = () => meeting.Join(userId, Clock.Now);

		// Then
		func.ShouldBrokeRule<CannotJoinToMeetingTwiceRule>();
	}
}