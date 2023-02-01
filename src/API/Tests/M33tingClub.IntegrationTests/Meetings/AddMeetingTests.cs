using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
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
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace M33tingClub.IntegrationTests.Meetings;

[TestFixture]
internal class AddMeetingTests : TestBase
{
    [Test]
    public async Task WhenAddMeetingWithOfficialTagsAndImage_ThenMeetingIsAddedWithImage()
    {
        // Given
        var userMax = TestAuthUsersProvider.MaxVerstappen;
        
        var uploadedImageId = await GivenBackgroundPhoto(userMax);
        var officialTagName = "officialTagName";
        await GivenOfficialTag(officialTagName, userMax);

        // When
        Clock.Set(new DateTimeOffset(2022, 05, 23, 9, 0, 0, TimeSpan.Zero));

        var meetingName = "MeetingName";
        var meetingDescription = "MeetingDescription";
        var participantsLimit = 5;
        var startDate = Clock.Now.AddHours(1);
        var endDate = Clock.Now.AddHours(13);
        var imageId = uploadedImageId;
        var locationName = "testLocation";
        var locationDescription = "testLocationDescription";
        var longitude = 13.441507496451164;
        var latitude = 52.47073169631442;
        var tags = new List<string> { "tag1", "tag2", officialTagName};
        var isPublic = true;
        var confidentialInfo = "info";

        var addMeetingCommand = new AddMeetingCommand(
            meetingName, 
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

        var meetingId = (await MeetingsClient.AddMeeting(addMeetingCommand, userMax)).Content!.Id;  
        // Then
        var userLongitude = 13.511833104962143;
        var userLatitude = 52.46040704221473;
        var response = await MeetingsClient.GetMeeting(meetingId, userLongitude, userLatitude, userMax);

        response.StatusCode.Should().Be(HttpStatusCode.OK); //TODO: Change that code

        var meeting = response.Content;
        
        meeting.Should().NotBeNull();
        meeting!.Name.Should().Be(meetingName);
        meeting.Description.Should().Be(meetingDescription);
        meeting.ParticipantsLimit.Should().Be(participantsLimit);
        meeting.StartDate.Should().Be(startDate);
        meeting.EndDate.Should().Be(endDate);
        meeting.ImageId.Should().Be(imageId.ToString());
        meeting.LocationName.Should().Be(locationName);
        meeting.LocationDescription.Should().Be(locationDescription);
        meeting.Longitude.Should().Be(longitude);
        meeting.Latitude.Should().Be(latitude);
        meeting.Distance.Should().BeGreaterThan(4).And.BeLessThan(5);
        meeting.Tags.Should().BeEquivalentTo(tags);
        meeting.Participants.Should().Contain(x => x.UserId == userMax.Id);
        meeting.ConfidentialInfo.Should().Be(confidentialInfo);
    }

    [Test]
    public async Task WhenAddMeetingWithoutParticipantsLimit_ThenMeetingIsAdded()
    {
        // Given
        var userMax = TestAuthUsersProvider.MaxVerstappen;
        
        var officialTagName = "officialTagName";
        await GivenOfficialTag(officialTagName, userMax);

        // When
        Clock.Set(new DateTimeOffset(2022, 05, 23, 9, 0, 0, TimeSpan.Zero));
        
        var meetingName = "MeetingName";
        var meetingDescription = "MeetingDescription";
        var participantsLimit = null as int?;
        var startDate = Clock.Now.AddHours(1);
        var endDate = Clock.Now.AddHours(13);
        var imageId = Guid.Parse("65BFC1CB-C4D1-442B-8B56-3EF03B6D5D3F");
        var locationName = "testLocation";
        var locationDescription = "testLocationDescription";
        var longitude = 13.441507496451164;
        var latitude = 52.47073169631442;
        var tags = new List<string> { "tag1", "tag2", officialTagName};
        var isPublic = true;
        var confidentialInfo = "info";

        var addMeetingCommand = new AddMeetingCommand(
            meetingName, 
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

        var meetingId = (await MeetingsClient.AddMeeting(addMeetingCommand, userMax)).Content!.Id;
        
        // Then
        var userLongitude = 13.511833104962143;
        var userLatitude = 52.46040704221473;
        var response = await MeetingsClient.GetMeeting(meetingId, userLongitude, userLatitude, userMax);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var meeting = response.Content;
        
        meeting.Should().NotBeNull();
        meeting!.Name.Should().Be(meetingName);
        meeting.Description.Should().Be(meetingDescription);
        meeting.ParticipantsLimit.Should().Be(participantsLimit);
        meeting.StartDate.Should().Be(startDate);
        meeting.EndDate.Should().Be(endDate);
        meeting.ImageId.Should().Be(imageId.ToString());
        meeting.LocationName.Should().Be(locationName);
        meeting.LocationDescription.Should().Be(locationDescription);
        meeting.Longitude.Should().Be(longitude);
        meeting.Latitude.Should().Be(latitude);
        meeting.Distance.Should().BeGreaterThan(4).And.BeLessThan(5);
        meeting.Tags.Should().BeEquivalentTo(tags);
        meeting.Participants.Should().BeEquivalentTo(new[]
        {
            new ParticipantDTO
            {
                UserId = userMax.Id,
                MeetingRole = MeetingRole.Owner.Name,
                Name = userMax.Name,
                ImageId = userMax.ImageId
            },
        });
        meeting.ConfidentialInfo.Should().Be(confidentialInfo);
    }

    [Test]
    //TODO: Check participants
    public async Task WhenAddMeetingWithoutOfficialTags_ThenMeetingIsNotAdded()
    {
        // Given
        var userMax = TestAuthUsersProvider.MaxVerstappen;
        
        // When
        Clock.Set(new DateTimeOffset(2022, 05, 23, 9, 0, 0, TimeSpan.Zero));
        
        var meetingName = "MeetingName";
        var meetingDescription = "MeetingDescription";
        var participantsLimit = 5;
        var startDate = Clock.Now.AddHours(1);
        var endDate = Clock.Now.AddHours(13);
        var imageId = Guid.Parse("1b1b7e93-4505-4d58-8b44-d81bfe4412aa");
        var locationName = "testLocation";
        var locationDescription = "testLocationDescription";
        var longitude = 13.441507496451164;
        var latitude = 52.47073169631442;
        var tags = new List<string> { "tag1", "tag2" };
        var isPublic = true;
        var confidentialInfo = "info";

        var addMeetingCommand = new AddMeetingCommand(
            meetingName, 
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

        var response = await MeetingsClient.AddMeeting(addMeetingCommand, userMax);

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        response.ErrorContent.Errors.Should().HaveCount(1);
        response.ErrorContent.Errors.Should().Contain("Meeting should have at least one official tag.");
    }

    private async Task GivenOfficialTag(string officialTagName, TestAuthUser? user = null)
    {
        await TagsClient.AddOfficialTag(new AddOfficialTagCommand(officialTagName), user);
    }

    private async Task<Guid> GivenBackgroundPhoto(TestAuthUser? user)
    {
        var image = new Image<Rgba32>(1000, 1000, Rgba32.ParseHex("#FF0000"));
        var stream = new MemoryStream();
        await image.SaveAsJpegAsync(stream);

        stream.Position = 0;
        var imageId = (await MeetingsClient.UploadBackgroundImage(stream, user)).Content.Id;
        return imageId;
    }
}
