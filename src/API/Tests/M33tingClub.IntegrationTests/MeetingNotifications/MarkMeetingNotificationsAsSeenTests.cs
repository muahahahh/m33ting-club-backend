using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using M33tingClub.Application.MeetingNotifications.MarkMeetingNotificationsAsSeen;
using M33tingClub.Application.Meetings.AddMeeting;
using M33tingClub.Application.Tags.AddOfficialTag;
using M33tingClub.Domain.Utilities;
using M33tingClub.IntegrationTests.Authentication;
using M33tingClub.IntegrationTests.Utilities;
using NUnit.Framework;

namespace M33tingClub.IntegrationTests.MeetingNotifications;

internal class MarkMeetingNotificationsAsSeenTests : TestBase
{
    [Test]
    public async Task GivenMeetingNotifications_WhenMarkAsSeen_ThenCorrectResultIsReturned()
    {
        // Given
        var sergioUser = TestAuthUsersProvider.SergioPerez;
        var landoUser = TestAuthUsersProvider.LandoNorris;
        var maxUser = TestAuthUsersProvider.MaxVerstappen;

        var officialTagName = "officialTagName";
        await GivenOfficialTag(officialTagName, sergioUser);
        
        var firstMeetingId = await GivenPublicMeeting(sergioUser, "firstMeeting", officialTagName);
        var secondMeetingId = await GivenPublicMeeting(sergioUser, "secondMeeting", officialTagName);
        
        await MeetingsClient.JoinMeeting(firstMeetingId, landoUser);
        await MeetingsClient.JoinMeeting(firstMeetingId, maxUser);
        await MeetingsClient.JoinMeeting(secondMeetingId, landoUser);
        await MeetingsClient.JoinMeeting(secondMeetingId, maxUser);

        var firstUnseenMeetingNotifications = (await UsersClient.GetUnseenMeetingNotifications(sergioUser.AuthToken)).Content;
        
        // When
        var notificationsIdsToMarkAsSeen = firstUnseenMeetingNotifications.Take(2).Select(x => x.Id).ToList();
        await UsersClient.MarkAsSeenMeetingNotifications(
            new MarkMeetingNotificationsAsSeenCommand(notificationsIdsToMarkAsSeen), sergioUser.AuthToken);

        // Then
        var secondUnseenMeetingNotifications = (await UsersClient.GetUnseenMeetingNotifications(sergioUser.AuthToken)).Content;
        secondUnseenMeetingNotifications.Select(x => x.Id).Should().BeEquivalentTo(new[]
        {
            firstUnseenMeetingNotifications[2].Id,
            firstUnseenMeetingNotifications[3].Id
        });
    }
    
    private async Task<Guid> GivenPublicMeeting(TestAuthUser user, string meetingName, string officialTagName)
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
    
    private async Task GivenOfficialTag(string officialTagName, TestAuthUser user)
    {
        await TagsClient.AddOfficialTag(new AddOfficialTagCommand(officialTagName), user);
    }
}