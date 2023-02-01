using System;
using M33tingClub.Domain.Meetings.Rules;
using M33tingClub.Domain.Users;
using M33tingClub.Domain.Utilities;
using M33tingClub.UnitTests.Utilities;
using NUnit.Framework;

namespace M33tingClub.UnitTests.Meetings.Meetings;

[TestFixture]
public class MeetingAskToJoinTests : TestBase
{
	[Test]
	public void GivenMeeting_WhenUserAsksToJoin_ThenUserApplicationIsCreated()
	{
		// Given
		var currentDate = new DateTimeOffset(2022, 5, 1, 9, 0, 0, DateTimeOffset.Now.Offset);
		Clock.Set(currentDate);
		var meeting = new MeetingBuilder().Build();

		// When
		var userId = UserId.CreateNew();
		var joinMeeting = () => meeting.AskToJoin(userId);

		// Then
		//TODO: Create event to check
	}

	[Test]
	public void GivenMeeting_AndUserAlreadyAsked_WhenUserAsksToJoin_ThenExceptionIsThrown()
	{
		// Given
		var currentDate = new DateTimeOffset(2022, 5, 10, 19, 0, 0, DateTimeOffset.Now.Offset);
		Clock.Set(currentDate);
		var meeting = new MeetingBuilder().Build();

		var userId = UserId.CreateNew();
		meeting.AskToJoin(userId);

		// When
		var joinMeeting = () => meeting.AskToJoin(userId);

		// Then
		joinMeeting.ShouldBrokeRule<CannotAskToJoinMeetingTwiceRule>();
	}
}