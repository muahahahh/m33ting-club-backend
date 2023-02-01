using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
internal class GetMeetingsForExploreTests : TestBase
{
    [Test]
    public async Task GivenMultipleMeetings_WhenGetMeetingsForExplore_ThenReceivedMeetingsExpectedByPagination()
    {
        // Given
        var landoUser = TestAuthUsersProvider.LandoNorris;
        
        Clock.Set(new DateTimeOffset(2022, 05, 24, 12, 0, 0, TimeSpan.Zero));
        
        var numberOfGeneratedMeetings = 15;
        await GivenMultipleMeetings(numberOfGeneratedMeetings, TestAuthUsersProvider.MaxVerstappen);

        // When
        var userLongitude = 2.234234;
        var userLatitude = 45.124234;
        var limit = 5;
        var offset = 0;

        var response = await MeetingsClient.GetMeetings(
            longitude: userLongitude,
            latitude: userLatitude,
            limit: limit,
            offset: offset,
            user: landoUser);
        
        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var pagingInfo = response.Content!;

        pagingInfo.Should().NotBeNull();
        pagingInfo.NumberOfRecords.Should().Be(limit);
        pagingInfo.TotalNumberOfRecords.Should().Be(numberOfGeneratedMeetings);
    }
    
    [Test]
    public async Task GivenMultipleMeetings_WhenGetMeetingsForExploreFromPageOneAndPageTwo_ThenAllMeetingsFromPageOneAreCloserThanMeetingsFromPageTwo()
    {
        // Given
        var landoUser = TestAuthUsersProvider.LandoNorris;
        
        Clock.Set(new DateTimeOffset(2022, 05, 24, 12, 0, 0, TimeSpan.Zero));
        
        var numberOfGeneratedMeetings = 15;
        await GivenMultipleMeetings(numberOfGeneratedMeetings, TestAuthUsersProvider.MaxVerstappen);

        // When
        var userLongitude = 2.234234;
        var userLatitude = 45.124234;
        var limit = 5;

        var firstPageMeetings = (await MeetingsClient.GetMeetings(
            longitude: userLongitude,
            latitude: userLatitude,
            limit: limit,
            offset: 0,
            user: landoUser)).Content!.Records;

        var secondPageMeetings = (await MeetingsClient.GetMeetings(
            longitude: userLongitude,
            latitude: userLatitude,
            limit: limit,
            offset: 5,
            user: landoUser)).Content!.Records;

        // Then
        firstPageMeetings.Should().HaveCount(limit);
        secondPageMeetings.Should().HaveCount(limit);

        var closestMeetingFromSecondPage = secondPageMeetings.OrderBy(x => x.Distance).First();
        
        firstPageMeetings.Should()
                .OnlyContain(meetingFromFirstPage =>
                             meetingFromFirstPage.Distance < closestMeetingFromSecondPage.Distance);
    }

    [Test]
    public async Task GivenMultipleMeetings_WhenGetMeetingsForExplore_AndFilterMeetingsBySameIdTags_ThenReceivedFilteredByTagsMeetings()
    {
        // Given
        var landoUser = TestAuthUsersProvider.LandoNorris;
        
        Clock.Set(new DateTimeOffset(2022, 05, 24, 12, 0, 0, TimeSpan.Zero));
        
        var numberOfMeetings = 5;
        var meetings = await GivenMultipleMeetings(numberOfMeetings, TestAuthUsersProvider.MaxVerstappen);

        // When
        var userLongitude = 2.234234;
        var userLatitude = 45.124234;
        var tags = new List<string>{ "tag1_1", "tag2_1" };

        var response = await MeetingsClient.GetMeetings(
            longitude: userLongitude,
            latitude: userLatitude,
            tags: tags,
            user: landoUser);

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var pagingInfo = response.Content;
        
        var allMeetings = pagingInfo.Records.Select(x => x.Id).ToList();

        pagingInfo.Should().NotBeNull();
        pagingInfo.NumberOfRecords.Should().Be(1);
        pagingInfo.TotalNumberOfRecords.Should().Be(1);
        allMeetings.Should().IntersectWith(meetings);
    }
    
    [Test]
    public async Task GivenMultipleMeetings_WhenGetMeetingsForExplore_AndFilterMeetingsByDifferentIdTags_ThenReceivedFilteredByTagsMeetings()
    {
        // Given
        var landoUser = TestAuthUsersProvider.LandoNorris;
        
        Clock.Set(new DateTimeOffset(2022, 05, 24, 12, 0, 0, TimeSpan.Zero));
        
        var numberOfMeetings = 5;
        var meetings = await GivenMultipleMeetings(numberOfMeetings, TestAuthUsersProvider.MaxVerstappen);

        // When
        var userLongitude = 2.234234;
        var userLatitude = 45.124234;
        var tags = new List<string>{ "tag1_1", "tag1_2" };

        var response = await MeetingsClient.GetMeetings(
            longitude: userLongitude,
            latitude: userLatitude,
            tags: tags,
            user: landoUser);

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var pagingInfo = response.Content;
        
        var allMeetings = pagingInfo.Records.Select(x => x.Id).ToList();

        pagingInfo.Should().NotBeNull();
        pagingInfo.NumberOfRecords.Should().Be(2);
        pagingInfo.TotalNumberOfRecords.Should().Be(2);
        allMeetings.Should().IntersectWith(meetings);
    }
    
    private async Task<List<Guid>> GivenMultipleMeetings(int numberOfMeetings, TestAuthUser user)
    {
        await GivenOfficialTag(user);
        
        var addedMeetingIds = new List<Guid>();

        for (var i = 0; i < numberOfMeetings; i++)
        {
            var meetingName = $"MeetingName_{i}";
            var meetingDescription = $"MeetingDescription_{i}";
            var participantsLimit = 5 + i;
            var imageId = Guid.NewGuid();
            var locationName = $"testLocation_{i}";
            var locationDescription = $"testLocationDescription_{i}";
            var latitude = 34.324234;
            var longitude = 175.423423 - 130.0 / (numberOfMeetings - i + 1);
            var tags = new List<string> {$"tag1_{i}", $"tag2_{i}", "officialTagName"};
            var isPublic = true;
            var confidentialInfo = "info";

            var addMeetingCommand = new AddMeetingCommand(
                meetingName,
                meetingDescription,
                participantsLimit,
                Clock.Now.AddDays(i).AddHours(1),
                Clock.Now.AddDays(i).AddHours(12), 
                imageId,
                locationName,
                locationDescription,
                latitude,
                longitude,
                tags,
                isPublic,
                confidentialInfo);
            
            var addedMeetingId = (await MeetingsClient.AddMeeting(addMeetingCommand, user)).Content!.Id;
            addedMeetingIds.Add(addedMeetingId);
        }

        return addedMeetingIds;
    }
    private async Task GivenOfficialTag(TestAuthUser user)
    {
        var officialTagName = "officialTagName";
        await TagsClient.AddOfficialTag(new AddOfficialTagCommand(officialTagName), user);
    }

    [Test]
    public async Task GivenMeetings_WhenGetMeetingsForExplore_ThenReceivedMeetingsInWhichIAmNotParticipating()
    {
        // Given
        var landoUser = TestAuthUsersProvider.LandoNorris;
        
        Clock.Set(new DateTimeOffset(2022, 05, 24, 12, 0, 0, TimeSpan.Zero));

        await GivenOfficialTag(landoUser);

        var participatingMeetingId = await GivenSingleMeeting(TestAuthUsersProvider.MaxVerstappen);
        await MeetingsClient.JoinMeeting(participatingMeetingId, landoUser);
        
        var otherMeeting1 = await GivenSingleMeeting(TestAuthUsersProvider.MaxVerstappen);
        var otherMeeting2 = await GivenSingleMeeting(TestAuthUsersProvider.SergioPerez);
        
        // When
        var userLongitude = 2.234234;
        var userLatitude = 45.124234;
        var limit = 5;
        var offset = 0;

        var response = await MeetingsClient.GetMeetings(
            longitude: userLongitude,
            latitude: userLatitude,
            limit: limit,
            offset: offset,
            user: landoUser);
        
        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var pagingInfo = response.Content!;

        pagingInfo.Should().NotBeNull();
        pagingInfo.NumberOfRecords.Should().Be(2);
        pagingInfo.TotalNumberOfRecords.Should().Be(2);

        pagingInfo.Records.Select(x => x.Id).Should().BeEquivalentTo(new[]
        {
            otherMeeting1,
            otherMeeting2
        });
    }
    
    private async Task<Guid> GivenSingleMeeting(TestAuthUser user)
    {
        var meetingName = "MeetingName";
        var meetingDescription = "MeetingDescription";
        var participantsLimit = 5;
        var imageId = Guid.NewGuid();
        var locationName = $"testLocation";
        var locationDescription = $"testLocationDescription";
        var latitude = 34.324234;
        var longitude = 175.423423;
        var tags = new List<string> {"officialTagName"};
        var isPublic = true;
        var confidentialInfo = "info";

        var addMeetingCommand = new AddMeetingCommand(
            meetingName,
            meetingDescription,
            participantsLimit,
            Clock.Now.AddDays(1).AddHours(1),
            Clock.Now.AddDays(2).AddHours(12), 
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
}