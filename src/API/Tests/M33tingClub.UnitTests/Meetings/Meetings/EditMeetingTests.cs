using System;
using System.Collections.Generic;
using M33tingClub.Domain.Meetings;
using M33tingClub.Domain.Meetings.Rules;
using M33tingClub.Domain.Tags;
using M33tingClub.Domain.Users;
using M33tingClub.UnitTests.Utilities;
using NUnit.Framework;

namespace M33tingClub.UnitTests.Meetings.Meetings;

[TestFixture]
public class EditMeetingTests
{
	[Test]
	public void GivenMeetingData_WithStartDateInThePast_WhenCreateMeeting_ThenExceptionIsThrown()
	{
		// Given
		var currentDate = new DateTimeOffset(2022, 6, 1, 9, 0, 0, DateTimeOffset.Now.Offset);
		var pictureId = Guid.NewGuid(); 
		
		var meetingId = MeetingId.FromGuid(Guid.Parse("94D64F8E-4F77-4704-AEAC-E805198FB42D"));
		var meetingName = "MeetingName";
		var meetingDescription = "MeetingDescription";
		var participantsLimit = 5;
		var startDate = currentDate.AddHours(1);
		var endDate = currentDate.AddHours(3);
		var picture = pictureId;
		var locationName = "testLocation";
		var locationDescription = "testLocationDescription";
		var longitude = 13.441507496451164;
		var latitude = 52.47073169631442;
		var tags = new List<TagInfo> { TagInfo.Create(TagName.Create("tag1")), TagInfo.Create(TagName.Create("tag2"), true) };
		var userId = UserId.FromGuid(Guid.Parse("ED23FD87-E493-4994-BCF7-A018972B84ED"));
		var isPublic = true;
		var confidentialInfo = null as string;
		
		var meeting = Meeting.Create(
			meetingId,
			meetingName,
			meetingDescription,
			participantsLimit,
			startDate,
			endDate,
			picture,
			locationName,
			locationDescription,
			longitude,
			latitude,
			tags,
			userId,
			currentDate,
			isPublic,
			confidentialInfo);
		
		// When
		var newStartDate = currentDate.AddHours(-1);
		
		var func = () => meeting.Edit(
			meetingName,
			meetingDescription,
			participantsLimit,
			newStartDate,
			endDate,
			picture,
			locationName,
			locationDescription,
			longitude,
			latitude,
			tags,
			userId,
			currentDate,
			isPublic,
			confidentialInfo);

		// Then
		func.ShouldBrokeRule<StartDateOfMeetingCannotBeInThePastRule>();
	}
}