using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using M33tingClub.Application.MeetingApplications;
using M33tingClub.Application.Meetings;
using M33tingClub.Application.Meetings.AddMeeting;
using M33tingClub.Application.Tags.AddOfficialTag;
using M33tingClub.Domain.Meetings;
using M33tingClub.Domain.Utilities;
using M33tingClub.IntegrationTests.Authentication;
using M33tingClub.IntegrationTests.Utilities;
using NUnit.Framework;

namespace M33tingClub.IntegrationTests.MeetingApplications;

[TestFixture]
internal class RejectMeetingApplicationsTests : TestBase
{
	[Test]
	public async Task GivenPrivateMeetingWithOneApplication_WhenOwnerRejectsApplication_ThenApplicationIsRejected()
	{
		// Given
		var sergioUser = TestAuthUsersProvider.SergioPerez;

		var applicant = TestAuthUsersProvider.LandoNorris;
		var applicants = new List<TestAuthUser>()
		{
			applicant
		};
		
		var meetingId = await GivenPrivateMeetingWithAnApplication(sergioUser, applicants);
		// When
		var response = await MeetingApplicationsClient.RejectApplication(meetingId, applicant.Id, sergioUser);
		
		// Then
		response.StatusCode.Should().Be(HttpStatusCode.OK);
		
		var applicationsResponse = await MeetingApplicationsClient.GetApplications(meetingId, sergioUser);
		applicationsResponse.Content!.Should().BeEquivalentTo(new []
		{
			new ApplicationDTO
				{
					MeetingId = meetingId,
					UserId = applicant.Id,
					Status = MeetingApplicationStatus.Rejected.Name,
					Name = applicant.Name,
					ImageId = applicant.ImageId
				}
		});

		var userLongitude = 13.511833104962143;
		var userLatitude = 52.46040704221473;
		var meetingResponse = await MeetingsClient.GetMeeting(meetingId, userLongitude, userLatitude, sergioUser);
		meetingResponse.Content!.Participants.Should().BeEquivalentTo(new[]
		{
			new ParticipantDTO
			{
				UserId = sergioUser.Id,
				MeetingRole = MeetingRole.Owner.Name,
				Name = sergioUser.Name,
				ImageId = sergioUser.ImageId
			}
		});
	}
	
	[Test]
	public async Task GivenPrivateMeetingWithOneApplication_WhenMemberRejectsApplication_ThenExceptionIsThrown()
	{
		// Given
		var sergioUser = TestAuthUsersProvider.SergioPerez;

		var applicant = TestAuthUsersProvider.LandoNorris;
		var applicants = new List<TestAuthUser>()
		{
			applicant
		};
		
		var meetingId = await GivenPrivateMeetingWithAnApplication(sergioUser, applicants);
		// When
		var response = await MeetingApplicationsClient.AcceptApplication(meetingId, applicant.Id, applicant);
		
		// Then
		response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		
		var applicationsResponse = await MeetingApplicationsClient.GetApplications(meetingId, sergioUser);
		applicationsResponse.Content!.Should().BeEquivalentTo(new []
		{
			new ApplicationDTO
			{
				MeetingId = meetingId,
				UserId = applicant.Id,
				Status = MeetingApplicationStatus.Pending.Name,
				Name = applicant.Name,
				ImageId = applicant.ImageId
			}
		});

		var userLongitude = 13.511833104962143;
		var userLatitude = 52.46040704221473;
		var meetingResponse = await MeetingsClient.GetMeeting(meetingId, userLongitude, userLatitude, sergioUser);
		meetingResponse.Content!.Participants.Should().BeEquivalentTo(new[]
		{
			new ParticipantDTO
			{
				UserId = sergioUser.Id,
				MeetingRole = MeetingRole.Owner.Name,
				Name = sergioUser.Name,
				ImageId = sergioUser.ImageId
			}
		});
	}

	private async Task<Guid> GivenPrivateMeetingWithAnApplication(TestAuthUser actingUser,List<TestAuthUser> applicants)
	{
		Clock.Set(new DateTimeOffset(2022, 05, 24, 12, 0, 0, TimeSpan.Zero));

		var addedMeetingId = await GivenPrivateMeeting(actingUser);
		
		foreach (var applicant in applicants)
		{
			await MeetingsClient.JoinMeeting(addedMeetingId, applicant);
		}

		return addedMeetingId;
	}

	private async Task<Guid> GivenPrivateMeeting(TestAuthUser user)
	{
		var officialTagName = "officialTagName";
		await GivenOfficialTag(officialTagName, user);
        
		var meetingName = "MeetingName";
		var meetingDescription = "MeetingDescription";
		var participantsLimit = 5;
		var startDate = Clock.Now.AddHours(1);
		var endDate = Clock.Now.AddHours(3);
		var imageId = Guid.Parse("1b1b7e93-4505-4d58-8b44-d81bfe4412aa");
		var locationName = "testLocation";
		var locationDescription = "testLocation Description";
		var latitude = 34.324234;
		var longitude = 23.423423;
		var tags = new List<string> {"tag1", "tag2", officialTagName};
		var isPublic = false;
		var confidentialInfo = "info2";

		var addMeetingCommand = new AddMeetingCommand(meetingName,
			meetingDescription,
			participantsLimit,
			startDate,
			endDate,
			imageId,
			locationName,
			locationDescription,
			longitude,
			latitude,
			tags,
			isPublic,
			confidentialInfo);
		
		return (await MeetingsClient.AddMeeting(addMeetingCommand, user)).Content!.Id;
	}
    
	private async Task GivenOfficialTag(string officialTagName, TestAuthUser user)
	{
		await TagsClient.AddOfficialTag(new AddOfficialTagCommand(officialTagName), user);
	}
}