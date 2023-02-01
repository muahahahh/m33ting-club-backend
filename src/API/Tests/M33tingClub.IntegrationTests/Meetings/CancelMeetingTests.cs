using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using M33tingClub.Application.Meetings.AddMeeting;
using M33tingClub.Application.Meetings.CancelMeeting;
using M33tingClub.Application.Tags.AddOfficialTag;
using M33tingClub.Domain.Meetings;
using M33tingClub.Domain.Utilities;
using M33tingClub.IntegrationTests.Authentication;
using M33tingClub.IntegrationTests.Utilities;
using NUnit.Framework;

namespace M33tingClub.IntegrationTests.Meetings;

[TestFixture]
internal class CancelMeetingTests : TestBase
{
    [Test]
    public async Task GivenMeeting_WhenCancelMeeting_ThenMeetingStatusIsCancelled()
    {
        // Given
        var landoUser = TestAuthUsersProvider.LandoNorris;
        
        Clock.Set(new DateTimeOffset(2022, 04, 24, 12, 0, 0, TimeSpan.Zero));

        var addedMeetingId = await GivenAddedMeeting(landoUser);

        // When
        await MeetingsClient.CancelMeeting(addedMeetingId, landoUser);

        // Then
        var response = await MeetingsClient.GetMeeting(addedMeetingId, user: landoUser);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var cancelledMeeting = response.Content;

        cancelledMeeting.Should().NotBeNull();
        cancelledMeeting!.Status.Should().Be(MeetingStatus.Cancelled.Name);
    }

    [Test]
    public async Task WhenCancelNotExistingMeeting_ThenMeetingNotFoundIsReturned()
    {
        // When
        var sergioUser = TestAuthUsersProvider.SergioPerez;

        var notExistingMeetingId = Guid.Parse("DA536400-8C49-4999-BC1F-E0D95F8DC073");
        
        var response = await MeetingsClient.CancelMeeting(notExistingMeetingId, sergioUser);

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        response.ErrorContent!.Errors.Should().HaveCount(1);
        response.ErrorContent!.Errors.Should().Contain($"{nameof(Meeting)} with Id: {notExistingMeetingId} not found.");
    }

    //TODO: Test when user is participant

    //TODO: Test when user is leader

    private async Task<Guid> GivenAddedMeeting(TestAuthUser user)
    {
        await GivenOfficialTag(user);
        
        var meetingName = "MeetingName";
        var meetingDescription = "MeetingDescription";
        var participantsLimit = 5;
        var startDate = Clock.Now.AddHours(4);
        var endDate = Clock.Now.AddHours(5);
        var imageId = Guid.NewGuid();
        var locationName = "testLocation";
        var locationDescription = "testLocationDescription";
        var longitude = 23.423423;
        var latitude = 34.324234;
        var tags = new List<string> {"tag1", "tag2", "officialTagName"};
        var isPublic = true;
        var confidentialInfo = "info";
        
        var tagName = "tag1";
        var addOfficialTagCommand = new AddOfficialTagCommand(tagName);
        await TagsClient.AddOfficialTag(addOfficialTagCommand, user);

        var addMeetingCommand = new AddMeetingCommand(meetingName,
            meetingDescription,
            participantsLimit,
            startDate,
            endDate,
            imageId,
            locationName,
            locationDescription,
            latitude,
            longitude,
            tags,
            isPublic,
            confidentialInfo);

        return (await MeetingsClient.AddMeeting(addMeetingCommand, user)).Content!.Id;
    }
    
    private async Task GivenOfficialTag(TestAuthUser user)
    {
        var officialTagName = "officialTagName";
        await TagsClient.AddOfficialTag(new AddOfficialTagCommand(officialTagName), user);
    }
}