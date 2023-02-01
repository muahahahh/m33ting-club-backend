using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using M33tingClub.Application.Meetings.AddMeeting;
using M33tingClub.Application.Meetings.EditMeeting;
using M33tingClub.Application.Tags.AddOfficialTag;
using M33tingClub.Domain.Meetings;
using M33tingClub.Domain.Utilities;
using M33tingClub.IntegrationTests.Authentication;
using M33tingClub.IntegrationTests.Utilities;
using NUnit.Framework;

namespace M33tingClub.IntegrationTests.Meetings;

[TestFixture]
internal class EditMeetingTests : TestBase
{
    //TODO: Add test when user is participant
    //TODO: Add test when user is leader
    [Test]
    public async Task GivenMeeting_WhenEditMeeting_AndEditorIsOwner_ThenMeetingIsEdited()
    {
        // Given
        var maxUser = TestAuthUsersProvider.MaxVerstappen;
        
        Clock.Set(new DateTimeOffset(2022, 05, 23, 12, 0, 0, TimeSpan.Zero));
        
        var addedMeetingId = await GivenAddedMeeting(maxUser);

        // When
        Clock.Set(new DateTimeOffset(2022, 05, 25, 12, 0, 0, TimeSpan.Zero));

        var newMeetingName = "MeetingNameNew";
        var newMeetingDescription = "MeetingDescriptionNew";
        var newParticipantsLimit = 10;
        var newStartDate = Clock.Now.AddHours(5);
        var newEndDate = Clock.Now.AddHours(15);
        var newImageId = Guid.Parse("1b1b7e93-4505-4d58-8b44-d81bfe4412bb");
        var newLocationName = "testLocationNew";
        var newLocationDescription = "testLocationDescriptioNew";
        var newLongitude = 24.423423;
        var newLatitude = 35.324234;
        var newTags = new List<string> {"newTag1", "newTag2", "officialTagName"};
        var isPublic = true;
        var confidentialInfo = "info";

        var editMeetingCommand = new EditMeetingCommand(addedMeetingId,
            newMeetingName,
            newMeetingDescription,
            newParticipantsLimit,
            newStartDate,
            newEndDate,
            newImageId,
            newLocationName,
            newLocationDescription,
            newLongitude,
            newLatitude,
            newTags,
            isPublic,
            confidentialInfo);

        await MeetingsClient.EditMeeting(editMeetingCommand, maxUser);

        // Then
        var response = await MeetingsClient.GetMeeting(addedMeetingId, user: maxUser);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var editedMeeting = response.Content;
        
        editedMeeting.Should().NotBeNull();
        editedMeeting!.Id.Should().Be(addedMeetingId);
        editedMeeting.Name.Should().Be(newMeetingName);
        editedMeeting.Description.Should().Be(newMeetingDescription);
        editedMeeting.ParticipantsLimit.Should().Be(newParticipantsLimit);
        editedMeeting.StartDate.Should().Be(newStartDate);
        editedMeeting.EndDate.Should().Be(newEndDate);
        editedMeeting.ImageId.Should().Be(newImageId);
        editedMeeting.LocationName.Should().Be(newLocationName);
        editedMeeting.LocationDescription.Should().Be(newLocationDescription);
        editedMeeting.Latitude.Should().Be(newLatitude);
        editedMeeting.Longitude.Should().Be(newLongitude);
        editedMeeting.Tags.Should().BeEquivalentTo(newTags);
        editedMeeting.ConfidentialInfo.Should().Be(confidentialInfo);
    }

    [Test]
    public async Task WhenEditNotExistingMeeting_ThenMeetingNotFoundIsReturned()
    {
        // Given
        var maxUser = TestAuthUsersProvider.MaxVerstappen;
        
        Clock.Set(new DateTimeOffset(2022, 05, 25, 12, 0, 0, TimeSpan.Zero));
        
        // When
        var newMeetingName = "MeetingNameNew";
        var newMeetingDescription = "MeetingDescriptionNew";
        var newParticipantsLimit = 10;
        var startDate = Clock.Now.AddHours(11);
        var endDate = Clock.Now.AddHours(13);
        var newImageId = Guid.Parse("1b1b7e93-4505-4d58-8b44-d81bfe4412aa");
        var newLocationName = "testLocationNew";
        var newLocationDescription = "testLocationDescriptioNew";
        var newLongitude = 24.423423;
        var newLatitude = 35.324234;
        var newTags = new List<string> {"newTag1", "newTag2"};
        var isPublic = true;
        var confidentialInfo = "info";

        var notExistingMeetingId = Guid.Parse("948A611A-F58B-45D9-898A-4CE146F2A302");

        var editMeetingCommand = new EditMeetingCommand(
            notExistingMeetingId,
            newMeetingName,
            newMeetingDescription,
            newParticipantsLimit,
            startDate,
            endDate,
            newImageId,
            newLocationName,
            newLocationDescription,
            newLongitude,
            newLatitude,
            newTags,
            isPublic,
            confidentialInfo);

        var response = await MeetingsClient.EditMeeting(editMeetingCommand, maxUser);

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        response.ErrorContent!.Errors.Should().HaveCount(1);
        response.ErrorContent!.Errors.Should().Contain($"{nameof(Meeting)} with Id: {notExistingMeetingId} not found.");
    }

    private async Task<Guid> GivenAddedMeeting(TestAuthUser user)
    {
        await GivenOfficialTag(user);
        
        var meetingName = "MeetingName";
        var meetingDescription = "MeetingDescription";
        var participantsLimit = 5;
        var startDate = Clock.Now.AddHours(9);
        var endDate = Clock.Now.AddHours(13);
        var imageId = Guid.Parse("1b1b7e93-4505-4d58-8b44-d81bfe4412aa");
        var locationName = "testLocation";
        var locationDescription = "testLocationDescription";
        var longitude = 23.423423;
        var latitude = 34.324234;
        var tags = new List<string> {"tag1", "tag2", "officialTagName"};
        var isPublic = true;
        var confidentialInfo = "info";
        
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
    
    private async Task GivenOfficialTag(TestAuthUser user)
    {
        var officialTagName = "officialTagName";
        var officialTagImageId = Guid.Parse("1b1b7e93-4505-4d58-8b44-d81bfe4412aa");
        await TagsClient.AddOfficialTag(new AddOfficialTagCommand(officialTagName), user);
    }
}