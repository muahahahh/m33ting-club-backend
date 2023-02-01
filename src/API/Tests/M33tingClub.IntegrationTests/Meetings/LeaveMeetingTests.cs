using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using M33tingClub.Application.Meetings;
using M33tingClub.Application.Meetings.AddMeeting;
using M33tingClub.Application.Tags.AddOfficialTag;
using M33tingClub.Domain.Meetings;
using M33tingClub.Domain.Utilities;
using M33tingClub.IntegrationTests.Authentication;
using M33tingClub.IntegrationTests.Utilities;
using NUnit.Framework;

namespace M33tingClub.IntegrationTests.Meetings;

[TestFixture]
internal class LeaveMeetingTests : TestBase
{
    [Test]
    public async Task GivenJoinedMeeting_WhenMemberLeavesMeeting_ThenMemberLeftMeeting()
    {
        // Given
        Clock.Set(new DateTimeOffset(2022, 05, 24, 12, 0, 0, TimeSpan.Zero));
        
        var sergioUser = TestAuthUsersProvider.SergioPerez;
        
        var addedMeetingId = await GivenMeeting(sergioUser);
        
        var landoUser = TestAuthUsersProvider.LandoNorris;
        
        await MeetingsClient.JoinMeeting(addedMeetingId, landoUser);
        
        // When
        await MeetingsClient.LeaveMeeting(addedMeetingId, landoUser);
        
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
        });
    }
    
    [Test]
    public async Task GivenJoinedMeeting_WhenOwnerLeavesMeeting_ThenOwnerDidNotLeaveMeeting()
    {
        // Given
        Clock.Set(new DateTimeOffset(2022, 05, 24, 12, 0, 0, TimeSpan.Zero));
        
        var sergioUser = TestAuthUsersProvider.SergioPerez;
        
        var addedMeetingId = await GivenMeeting(sergioUser);
        
        var landoUser = TestAuthUsersProvider.LandoNorris;
        
        await MeetingsClient.JoinMeeting(addedMeetingId, landoUser);
        
        // When
        await MeetingsClient.LeaveMeeting(addedMeetingId, sergioUser);
        
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
    }
    
    [Test]
    public async Task GivenJoinedMeeting_WhenMemberLeavesMeeting_ThenMemberLeftMeeting_AndApplicationIsRemoved()
    {
        // Given
        Clock.Set(new DateTimeOffset(2022, 05, 24, 12, 0, 0, TimeSpan.Zero));
        
        var sergioUser = TestAuthUsersProvider.SergioPerez;
        
        var addedMeetingId = await GivenMeeting(sergioUser, false);
        
        var landoUser = TestAuthUsersProvider.LandoNorris;
        
        await MeetingsClient.JoinMeeting(addedMeetingId, landoUser);
        await MeetingApplicationsClient.AcceptApplication(addedMeetingId, landoUser.Id, sergioUser);

        // When
        await MeetingsClient.LeaveMeeting(addedMeetingId, landoUser);

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
        });
        
        var applications = 
            (await MeetingApplicationsClient.GetApplications(addedMeetingId, landoUser)).Content;
        applications.Should().BeEmpty();
    }

    private async Task<Guid> GivenMeeting(TestAuthUser user, bool isPublic = true)
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
        var confidentialInfo = "info3";

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