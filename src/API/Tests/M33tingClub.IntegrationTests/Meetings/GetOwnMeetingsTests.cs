using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using M33tingClub.Application.Meetings.AddMeeting;
using M33tingClub.Application.Meetings.CancelMeeting;
using M33tingClub.Application.Tags.AddOfficialTag;
using M33tingClub.Domain.Utilities;
using M33tingClub.IntegrationTests.Authentication;
using M33tingClub.IntegrationTests.Utilities;
using NUnit.Framework;

namespace M33tingClub.IntegrationTests.Meetings;

[TestFixture]
internal class GetOwnMeetingsTests : TestBase
{
    [Test]
    public async Task GivenMultipleMeetings_WhenGetMeetings_ThenReceivedMeetingsExpectedByPagination()
    {
        // Given
        var maxUser = TestAuthUsersProvider.MaxVerstappen;
        
        Clock.Set(new DateTimeOffset(2022, 05, 24, 12, 0, 0, TimeSpan.Zero));
        
        var numberOfGeneratedMeetings = 15;
        await GivenMultipleMeetings(numberOfGeneratedMeetings, maxUser);

        // When
        var statuses = new List<string> {"Upcoming"};
        var limit = 5;
        var offset = 0;

        var response = await MeetingsClient.GetOwnMeetings(
            statuses: statuses,
            limit: limit,
            offset: offset,
            user: maxUser);
        
        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var meetings = response.Content!.ToList();

        meetings.Should().NotBeNull();
        meetings.Should().HaveCount(limit);
    }
    
    [Test]
    public async Task GivenMultipleUpcomingMeetings_WhenGetUpcomingMeetings_ThenReceivedUpcomingMeetings()
    {
        // Given
        var maxUser = TestAuthUsersProvider.MaxVerstappen;
        
        Clock.Set(new DateTimeOffset(2022, 05, 24, 12, 0, 0, TimeSpan.Zero));
        
        var numberOfGeneratedMeetings = 15;
        var allMeetings = await GivenMultipleMeetings(numberOfGeneratedMeetings, maxUser);

        // When
        var statuses = new List<string> {"Upcoming"};
        var limit = 15;
        var offset = 0;

        var response = await MeetingsClient.GetOwnMeetings(
            statuses: statuses,
            limit: limit,
            offset: offset,
            user: maxUser);
        
        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var meetings = response.Content!.ToList();

        meetings.Should().NotBeNull();
        meetings.Should().HaveCount(numberOfGeneratedMeetings);
        meetings.Select(x => x.Id).ToList().Should().BeEquivalentTo(allMeetings);
    }
    
    [Test]
    public async Task GivenMultipleCancelledMeetings_WhenGetCancelledMeetings_ThenReceivedCancelledMeetings()
    {
        // Given
        var maxUser = TestAuthUsersProvider.MaxVerstappen;
        
        Clock.Set(new DateTimeOffset(2022, 05, 24, 12, 0, 0, TimeSpan.Zero));
        
        var numberOfGeneratedMeetings = 15;
        var allMeetings = await GivenMultipleMeetings(numberOfGeneratedMeetings, maxUser);
        var numberOfCancelledMeetings = 5;
        var cancelledMeetings = await CancelMultipleMeetings(numberOfCancelledMeetings, allMeetings, maxUser);

        // When
        var statuses = new List<string> {"Cancelled"};
        var limit = 15;
        var offset = 0;

        var response = await MeetingsClient.GetOwnMeetings(
            statuses: statuses,
            limit: limit,
            offset: offset,
            user: maxUser);
        
        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var meetings = response.Content!.ToList();

        meetings.Should().NotBeNull();
        meetings.Should().HaveCount(numberOfCancelledMeetings);
        meetings.Select(x => x.Id).ToList().Should().BeEquivalentTo(cancelledMeetings);
    }
    
    [Test]
    [Ignore("Check")]
    public async Task GivenMultipleUpcomingMeetings_WhenGetUpcomingMeetingsFromPageOneAndPageTwo_ThenAllUpcomingMeetingsFromPageOneAreEarlierThanUpcomingMeetingsFromPageTwo()
    {
        // Given
        var maxUser = TestAuthUsersProvider.MaxVerstappen;
        
        Clock.Set(new DateTimeOffset(2022, 05, 24, 12, 0, 0, TimeSpan.Zero));
        
        var numberOfGeneratedMeetings = 15;
        await GivenMultipleMeetings(numberOfGeneratedMeetings, maxUser);

        // When
        var statuses = new List<string> {"Upcoming"};
        var limit = 5;

        var firstPageMeetings = (await MeetingsClient.GetOwnMeetings(
            statuses: statuses,
            limit: limit,
            offset: 0,
            user: maxUser)).Content!.ToList();

        var secondPageMeetings = (await MeetingsClient.GetOwnMeetings(
            statuses: statuses,
            limit: limit,
            offset: 5,
            user: maxUser)).Content!.ToList();
        
        // Then
        firstPageMeetings.Should().HaveCount(limit);
        secondPageMeetings.Should().HaveCount(limit);

        var closestMeetingFromSecondPage = secondPageMeetings.OrderBy(x => x.Distance).First();
        
        firstPageMeetings.Should()
            .OnlyContain(meetingFromFirstPage =>
                meetingFromFirstPage.StartDate < closestMeetingFromSecondPage.StartDate);
    }

    private async Task<List<Guid>> GivenMultipleMeetings(int numberOfMeetings, TestAuthUser user)
    {
        await GivenOfficialTag(user);
        
        var addedMeetings = new List<Guid>();

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
            
            var addedMeeting = (await MeetingsClient.AddMeeting(addMeetingCommand, user)).Content!.Id;
            addedMeetings.Add(addedMeeting);
        }
        
        addedMeetings.Reverse();
        return addedMeetings;
    }
    
    [Test]
    public async Task GivenMultipleUpcomingAndCancelledMeetings_WhenGetUpcomingAndCancelledMeetings_ThenReceivedUpcomingMeetings()
    {
        // Given
        var maxUser = TestAuthUsersProvider.MaxVerstappen;
        
        Clock.Set(new DateTimeOffset(2022, 05, 24, 12, 0, 0, TimeSpan.Zero));
        
        var numberOfGeneratedMeetings = 15;
        var allMeetings = await GivenMultipleMeetings(numberOfGeneratedMeetings, maxUser);
        
        var numberOfCancelledMeetings = 5;
        await CancelMultipleMeetings(numberOfCancelledMeetings, allMeetings, maxUser);

        // When
        var statuses = new List<string> {"Upcoming", "Cancelled"};
        var limit = 15;
        var offset = 0;

        var response = await MeetingsClient.GetOwnMeetings(
            statuses: statuses,
            limit: limit,
            offset: offset,
            user: maxUser);
        
        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var meetings = response.Content!.ToList();

        meetings.Should().NotBeNull();
        meetings.Should().HaveCount(numberOfGeneratedMeetings);
        meetings.Select(x => x.Id).ToList().Should().BeEquivalentTo(allMeetings);
    }

    private async Task<List<Guid>> CancelMultipleMeetings(
        int numberOfMeetingsToCancel, 
        List<Guid> meetings,
        TestAuthUser user)
    {
        var canceledMeetingIds = new List<Guid>();
        for (int i = 0; i < numberOfMeetingsToCancel; i++)
        {
            var meetingToCancel = meetings[i];
            await MeetingsClient.CancelMeeting(meetingToCancel, user);
            canceledMeetingIds.Add(meetingToCancel);
        }
        return canceledMeetingIds;
    }
    
    private async Task GivenOfficialTag(TestAuthUser user)
    {
        var officialTagName = "officialTagName";
        var officialTagImageId = Guid.Parse("1b1b7e93-4505-4d58-8b44-d81bfe4412aa");
        await TagsClient.AddOfficialTag(new AddOfficialTagCommand(officialTagName), user);
    }
    
    
}