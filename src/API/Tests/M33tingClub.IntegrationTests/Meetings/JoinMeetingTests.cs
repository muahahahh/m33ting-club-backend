using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using M33tingClub.Application.MeetingApplications;
using M33tingClub.Application.MeetingNotifications.GetUnseenMeetingNotifications;
using M33tingClub.Application.Meetings;
using M33tingClub.Application.Meetings.AddMeeting;
using M33tingClub.Application.Tags.AddOfficialTag;
using M33tingClub.Domain.MeetingNotifications;
using M33tingClub.Domain.Meetings;
using M33tingClub.Domain.Utilities;
using M33tingClub.IntegrationTests.Authentication;
using M33tingClub.IntegrationTests.Utilities;
using NUnit.Framework;

namespace M33tingClub.IntegrationTests.Meetings;

[TestFixture]
internal class JoinMeetingTests : TestBase
{
	[Test]
	public async Task GivenPublicMeetingWithAvailableCapacity_WhenJoinPublicMeetingWithAvailableCapacity_ThenUserJoined()
	{
		// Given
		var sergioUser = TestAuthUsersProvider.SergioPerez;
		
		Clock.Set(new DateTimeOffset(2022, 05, 24, 12, 0, 0, TimeSpan.Zero));

		var addedMeetingId = await GivenPublicMeeting(sergioUser);

		// When
		var landoUser = TestAuthUsersProvider.LandoNorris;

		await MeetingsClient.JoinMeeting(addedMeetingId, landoUser);

		// Then   
		var meetingResponse = await MeetingsClient.GetMeeting(addedMeetingId, user: landoUser);
		meetingResponse.Content!.Participants.Should().BeEquivalentTo(new[]
		{
			new ParticipantDTO
			{
				UserId = sergioUser.Id, 
				MeetingRole = MeetingRole.Owner.Name,
				Name = sergioUser.Name,
				ImageId = sergioUser.ImageId
			},
			new ParticipantDTO
			{
				UserId = landoUser.Id, 
				MeetingRole = MeetingRole.Member.Name,
				Name = landoUser.Name,
				ImageId = landoUser.ImageId
			}
		});

		var meetingNotifications = await UsersClient.GetUnseenMeetingNotifications(sergioUser.AuthToken);
		meetingNotifications.Content.Should().ContainEquivalentOf(new MeetingNotificationDTO
		{
			Type = MeetingNotificationType.UserJoined.Name,
			MeetingId = addedMeetingId,
			PerformerId = landoUser.Id,
			PerformerName = landoUser.Name
		}, options => options
			.Excluding(x => x.Id)
			.Excluding(x => x.OccuredOn));
	}
	
	[Test]
	public async Task GivenPublicMeetingWithNoCapacity_WhenJoinPublicMeetingWithNoCapacity_ThenThenExceptionIsThrown()
	{
		// Given
		var sergioUser = TestAuthUsersProvider.SergioPerez;
		
		Clock.Set(new DateTimeOffset(2022, 05, 24, 12, 0, 0, TimeSpan.Zero));

		var addedMeetingId = await GivenPublicMeeting(sergioUser);
		var landoUser = TestAuthUsersProvider.LandoNorris;
		await MeetingsClient.JoinMeeting(addedMeetingId, landoUser);
		
		// When
		var maxUser = TestAuthUsersProvider.MaxVerstappen;
		var joinResponse = await MeetingsClient.JoinMeeting(addedMeetingId, maxUser);
		
		// Then
		joinResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		var meetingResponse = await MeetingsClient.GetMeeting(addedMeetingId, user: landoUser);
		meetingResponse.Content!.Participants.Should().BeEquivalentTo(new[]
		{
			new ParticipantDTO
			{
				UserId = sergioUser.Id, 
				MeetingRole = MeetingRole.Owner.Name,
				Name = sergioUser.Name,
				ImageId = sergioUser.ImageId
			},
			new ParticipantDTO
			{
				UserId = landoUser.Id, 
				MeetingRole = MeetingRole.Member.Name,
				Name = landoUser.Name,
				ImageId = landoUser.ImageId
			}
		});
	}
	
	[Test]
	public async Task GivenPublicMeetingWithAvailableCapacity_WhenJoinPublicMeetingWithAvailableCapacity_ThenNoPendingApplicationIsCreated()
	{
		// Given
		var sergioUser = TestAuthUsersProvider.SergioPerez;
		
		Clock.Set(new DateTimeOffset(2022, 05, 24, 12, 0, 0, TimeSpan.Zero));

		var addedMeetingId = await GivenPublicMeeting(sergioUser);

		// When
		var landoUser = TestAuthUsersProvider.LandoNorris;

		var response = await MeetingsClient.JoinMeeting(addedMeetingId, landoUser);

		// Then   
		response.StatusCode.Should().Be(HttpStatusCode.NoContent);
		var applicationsResponse = await MeetingApplicationsClient.GetApplications(addedMeetingId, sergioUser);
		applicationsResponse.Content!.Should().HaveCount(0);
	}

	[Test]
	public async Task GivenPrivateMeetingWithAvailableCapacity_WhenJoinPrivateMeetingWithAvailableCapacity_ThenApplicationIsPending()
	{
		// Given
		var sergioUser = TestAuthUsersProvider.SergioPerez;
		
		Clock.Set(new DateTimeOffset(2022, 05, 24, 12, 0, 0, TimeSpan.Zero));

		var addedMeetingId = await GivenPrivateMeeting(sergioUser);

		// When
		var landoUser = TestAuthUsersProvider.LandoNorris;

		await MeetingsClient.JoinMeeting(addedMeetingId, landoUser);

		// Then   
		var meetingResponse = await MeetingsClient.GetMeeting(addedMeetingId, user: landoUser);
		meetingResponse.Content!.Participants.Should().NotContainEquivalentOf(
			new ParticipantDTO
			{
				UserId = landoUser.Id, 
				MeetingRole = MeetingRole.Member.Name,
				Name = landoUser.Name,
				ImageId = landoUser.ImageId
			}
		);

		var applicationsResponse = await MeetingApplicationsClient.GetApplications(addedMeetingId, sergioUser);
		applicationsResponse.StatusCode.Should().Be(HttpStatusCode.OK);
		applicationsResponse.Content!.Should().ContainEquivalentOf(
			new ApplicationDTO
			{
				MeetingId = addedMeetingId,
				UserId = landoUser.Id,
				Status = MeetingApplicationStatus.Pending.Name,
				Name = landoUser.Name,
				ImageId = landoUser.ImageId
			}
		);
		
		var meetingNotifications = await UsersClient.GetUnseenMeetingNotifications(sergioUser.AuthToken);
		meetingNotifications.Content.Should().ContainEquivalentOf(new MeetingNotificationDTO
		{
			Type = MeetingNotificationType.UserAskedToJoin.Name,
			MeetingId = addedMeetingId,
			PerformerId = landoUser.Id,
			PerformerName = landoUser.Name
		}, options => options
			.Excluding(x => x.Id)
			.Excluding(x => x.OccuredOn));
	}
	
	[Test]
	public async Task GivenPrivateMeetingWithNoCapacity_WhenJoinPrivateMeetingWithNoCapacity_ThenExceptionIsThrown()
	{
		// Given
		var sergioUser = TestAuthUsersProvider.SergioPerez;
		
		Clock.Set(new DateTimeOffset(2022, 05, 24, 12, 0, 0, TimeSpan.Zero));

		var addedMeetingId = await GivenPrivateMeeting(sergioUser);
		var landoUser = TestAuthUsersProvider.LandoNorris;
		await MeetingsClient.JoinMeeting(addedMeetingId, landoUser);
		await MeetingApplicationsClient.AcceptApplication(addedMeetingId, landoUser.Id, sergioUser);
		
		// When
		var maxUser = TestAuthUsersProvider.MaxVerstappen;
		var joinResponse = await MeetingsClient.JoinMeeting(addedMeetingId, maxUser);
		
		// Then
		joinResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		var meetingResponse = await MeetingsClient.GetMeeting(addedMeetingId, user: landoUser);
		meetingResponse.Content!.Participants.Should().BeEquivalentTo(new[]
		{
			new ParticipantDTO
			{
				UserId = sergioUser.Id, 
				MeetingRole = MeetingRole.Owner.Name,
				Name = sergioUser.Name,
				ImageId = sergioUser.ImageId
			},
			new ParticipantDTO
			{
				UserId = landoUser.Id, 
				MeetingRole = MeetingRole.Member.Name,
				Name = landoUser.Name,
				ImageId = landoUser.ImageId
			}
		});
	}
	
	private async Task<Guid> GivenPublicMeeting(TestAuthUser user)
	{
		var officialTagName = "officialTagName";
        await GivenOfficialTag(officialTagName, user);
        
        var meetingName = "MeetingName";
        var meetingDescription = "MeetingDescription";
        var participantsLimit = 2;
        var startDate = Clock.Now.AddHours(1);
        var endDate = Clock.Now.AddHours(3);
        var imageId = Guid.Parse("1b1b7e93-4505-4d58-8b44-d81bfe4412aa");
        var locationName = "testLocation";
        var locationDescription = "testLocation Description";
        var latitude = 34.324234;
        var longitude = 23.423423;
        var tags = new List<string> {"tag1", "tag2", officialTagName};
        var isPublic = true;
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
	
	private async Task<Guid> GivenPrivateMeeting(TestAuthUser user)
	{
		var officialTagName = "officialTagName";
		await GivenOfficialTag(officialTagName, user);
        
		var meetingName = "MeetingName";
		var meetingDescription = "MeetingDescription";
		var participantsLimit = 2;
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