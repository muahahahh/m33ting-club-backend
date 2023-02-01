using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using M33tingClub.Application.Meetings.AddMeeting;
using M33tingClub.Application.Tags.AddOfficialTag;
using M33tingClub.Domain.Meetings;
using M33tingClub.Domain.Utilities;
using M33tingClub.IntegrationTests.Authentication;
using M33tingClub.IntegrationTests.Utilities;
using NUnit.Framework;

namespace M33tingClub.IntegrationTests.Users;

internal class DeleteUserTests : TestBase
{
    [Test]
    public async Task GivenUser_WhenDeleteUser_ThenIsDeleted()
    {
        // Given
        Clock.Set(new DateTimeOffset(2022, 11, 21, 23, 23, 0, TimeSpan.Zero));
        var lewisHamilton = await AddUser();

        var lewisMeetingId = await GivenPrivateMeeting(lewisHamilton);

        // When
        await UsersClient.Delete(lewisHamilton.AuthToken);
        
        // Then
        var landoNorris = TestAuthUsersProvider.LandoNorris;

        var getUserResponse = await UsersClient.GetUser(landoNorris.AuthToken, lewisHamilton.Id);
        getUserResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var getMeetingResponse = await MeetingsClient.GetMeeting(lewisMeetingId, user: landoNorris);
        getMeetingResponse.Content.Status.Should().Be(MeetingStatus.Cancelled.Name);
    }

    private async Task<TestAuthUser> AddUser()
    {
        var lewisHamiltonCredentials = new TestUserCredentials(
            "lewis_hamilton@meet.com",
            "lewis_hamilton",
            "+48666666666",
            "Lewis Hamilton",
            new DateTime(1985, 1, 7, 0, 0, 0, DateTimeKind.Utc),
            "Male",
            Guid.Parse("B85FA583-784F-4797-A935-8E809B3F27E9"));

        var firebaseConfiguration = TestAuthCreator.ReadFirebaseConfiguration();
        var firebaseClient = new FirebaseClient(firebaseConfiguration.ApiKey);
        
        return await firebaseClient.AddToFirebaseAndAuthorize(lewisHamiltonCredentials, UsersClient);
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