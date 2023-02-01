using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using M33tingClub.Application.Meetings.AddMeeting;
using M33tingClub.Application.Tags.AddOfficialTag;
using M33tingClub.Domain.Utilities;
using M33tingClub.IntegrationTests.Authentication;
using M33tingClub.IntegrationTests.Utilities;
using NUnit.Framework;

namespace M33tingClub.IntegrationTests.Meetings;

[TestFixture]
internal class GetMeetingTests : TestBase
{
    [Test]
    public async Task GivenMeetingOwner_WhenGetMeeting_ThenSeesConfidentialInfo()
    {
        // Given
        var owner = TestAuthUsersProvider.LandoNorris;

        var confidentialInfo = "some discord link";

        var meetingId = await AddMeeting(confidentialInfo, owner);

        // When
        var meeting = await MeetingsClient.GetMeeting(meetingId, user: owner);

        // Then
        meeting.Content.ConfidentialInfo.Should().Be(confidentialInfo);
    }
    
    [Test]
    public async Task GivenMeetingParticipant_WhenGetMeeting_ThenSeesConfidentialInfo()
    {
        // Given
        var owner = TestAuthUsersProvider.LandoNorris;

        var confidentialInfo = "some discord link2";

        var meetingId = await AddMeeting(confidentialInfo, owner);

        var participant = TestAuthUsersProvider.MaxVerstappen;

        await MeetingsClient.JoinMeeting(meetingId, participant);
        
        // When
        var meeting = await MeetingsClient.GetMeeting(meetingId, user: participant);

        // Then
        meeting.Content.ConfidentialInfo.Should().Be(confidentialInfo);
    }
    
    [Test]
    public async Task GivenNotMeetingParticipant_WhenGetMeeting_ThenSeesConfidentialInfo()
    {
        // Given
        var owner = TestAuthUsersProvider.LandoNorris;

        var confidentialInfo = "some discord link3";

        var meetingId = await AddMeeting(confidentialInfo, owner);

        var notParticipant = TestAuthUsersProvider.MaxVerstappen;

        // When
        var meeting = await MeetingsClient.GetMeeting(meetingId, user: notParticipant);

        // Then
        meeting.Content.ConfidentialInfo.Should().Be(null);
    }

    private async Task<Guid> AddMeeting(string? confidentialInfo, TestAuthUser? user)
    {
        var officialTagName = "officialTagName";
        await TagsClient.AddOfficialTag(new AddOfficialTagCommand(officialTagName), user);
        
        var name = "MeetingName";
        var description = "MeetingDescription";
        var participantsLimit = 5;
        var startDate = Clock.Now.AddHours(1);
        var endDate = Clock.Now.AddHours(13);
        var imageId = Guid.Parse("1b1b7e93-4505-4d58-8b44-d81bfe4412aa");
        var locationName = "testLocation";
        var locationDescription = "testLocationDescription";
        var longitude = 13.441507496451164;
        var latitude = 52.47073169631442;
        var tags = new List<string> { officialTagName };
        var isPublic = true;

        var addMeetingCommand = new AddMeetingCommand(
            name, 
            description, 
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
        
        return (await MeetingsClient.AddMeeting(addMeetingCommand, user)).Content.Id;
    }
}