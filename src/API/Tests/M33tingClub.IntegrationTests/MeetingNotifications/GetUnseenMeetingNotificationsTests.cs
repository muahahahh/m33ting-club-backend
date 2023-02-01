using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using M33tingClub.Application.Meetings.AddMeeting;
using M33tingClub.Application.Tags.AddOfficialTag;
using M33tingClub.Domain.MeetingNotifications;
using M33tingClub.Domain.Utilities;
using M33tingClub.IntegrationTests.Authentication;
using M33tingClub.IntegrationTests.Utilities;
using NUnit.Framework;

namespace M33tingClub.IntegrationTests.MeetingNotifications;

internal class GetUnseenMeetingNotificationsTests : TestBase
{
    [Test]
    public async Task GivenMeetingNotifications_WhenGetUnseenMeetingNotifications_AndFiterByType_ThenCorrectResultIsReturned()
    {
        // Given
        var sergioUser = TestAuthUsersProvider.SergioPerez;
        var landoUser = TestAuthUsersProvider.LandoNorris;
        var maxUser = TestAuthUsersProvider.MaxVerstappen;

        var officialTagName = "officialTagName";
        await GivenOfficialTag(officialTagName, sergioUser);
        
        var publicMeetingId = await GivenMeeting(sergioUser, true,"firstMeeting", officialTagName);
        var privateMeetingId = await GivenMeeting(sergioUser, false,"secondMeeting", officialTagName);
        
        await MeetingsClient.JoinMeeting(publicMeetingId, landoUser);
        await MeetingsClient.JoinMeeting(publicMeetingId, maxUser);
        await MeetingsClient.JoinMeeting(privateMeetingId, landoUser);
        await MeetingsClient.JoinMeeting(privateMeetingId, maxUser);

        // When
        var unseenMeetingNotifications = (await UsersClient.GetUnseenMeetingNotifications(
            sergioUser.AuthToken, 
            new List<string>{ MeetingNotificationType.UserAskedToJoin.Name })).Content;
        
        // Then
        unseenMeetingNotifications.Should().HaveCount(2);
        unseenMeetingNotifications.Should().OnlyContain(x => x.Type == MeetingNotificationType.UserAskedToJoin.Name);
    }
    
    private async Task<Guid> GivenMeeting(TestAuthUser user, bool isPublic, string meetingName, string officialTagName)
    {
        var meetingDescription = "MeetingDescription";
        var participantsLimit = 10;
        var startDate = Clock.Now.AddHours(1);
        var endDate = Clock.Now.AddHours(3);
        var imageId = Guid.Parse("1b1b7e93-4505-4d58-8b44-d81bfe4412aa");
        var locationName = "testLocation";
        var locationDescription = "testLocation Description";
        var latitude = 34.324234;
        var longitude = 23.423423;
        var tags = new List<string> {"tag1", "tag2", officialTagName};
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