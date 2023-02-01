using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using M33tingClub.Application.Meetings.AddMeeting;
using M33tingClub.Application.Meetings.GetFeed;
using M33tingClub.Application.Tags.AddOfficialTag;
using M33tingClub.Domain.Meetings;
using M33tingClub.Domain.Utilities;
using M33tingClub.IntegrationTests.Authentication;
using M33tingClub.IntegrationTests.Utilities;
using NUnit.Framework;

namespace M33tingClub.IntegrationTests.Meetings;

[TestFixture]
internal class GetFeedTests : TestBase
{
    [Test]
    public async Task GivenUserWithoutFollowing_WhenGetFeed_ThenFeedIsEmpty()
    {
        // Given
        var userSearchingForFeed = TestAuthUsersProvider.LandoNorris;

        await UsersClient.FollowUser(userSearchingForFeed.Id, TestAuthUsersProvider.MaxVerstappen);
        await UsersClient.FollowUser(userSearchingForFeed.Id, TestAuthUsersProvider.SergioPerez);
        
        var tagName = await GivenOfficialTag(userSearchingForFeed);

        var meetingId = await AddMeeting("Any name", Guid.NewGuid(), tagName, TestAuthUsersProvider.SergioPerez);
        await MeetingsClient.JoinMeeting(meetingId, TestAuthUsersProvider.MaxVerstappen);

        // When
        var feed = (await MeetingsClient.GetFeed(user: userSearchingForFeed)).Content;

        // Then
        feed.NumberOfRecords.Should().Be(0);
        feed.TotalNumberOfRecords.Should().Be(0);
        feed.Records.Should().BeEmpty();
    }

    [Test]
    public async Task GivenUserWithFollowing_WhenGetFeed_ThenFeedIsReturned()
    {
        // Given
        var userSearchingForFeed = TestAuthUsersProvider.LandoNorris;

        await UsersClient.FollowUser(TestAuthUsersProvider.MaxVerstappen.Id, userSearchingForFeed);
        await UsersClient.FollowUser(TestAuthUsersProvider.SergioPerez.Id, userSearchingForFeed);
        
        var tagName = await GivenOfficialTag(userSearchingForFeed);

        var meetingName = "Eating some Martin's croissants";
        var meetingImageId = Guid.NewGuid();

        var createMeetingDate = new DateTimeOffset(2022, 11, 24, 12, 0, 0, TimeSpan.Zero);
        Clock.Set(createMeetingDate);
        var meetingId = await AddMeeting(meetingName, meetingImageId, tagName, TestAuthUsersProvider.SergioPerez);

        var joinMeetingDate = new DateTimeOffset(2022, 11, 24, 14, 0, 0, TimeSpan.Zero);
        Clock.Set(joinMeetingDate);
        await MeetingsClient.JoinMeeting(meetingId, TestAuthUsersProvider.MaxVerstappen);

        // When
        var feed = (await MeetingsClient.GetFeed(user: userSearchingForFeed)).Content;

        // Then
        feed.NumberOfRecords.Should().Be(2);
        feed.TotalNumberOfRecords.Should().Be(2);
        feed.Records.Should().BeInDescendingOrder(x => x.OccuredOn);
        feed.Records.Should().BeEquivalentTo(new[]
        {
            new FeedDTO
            {
                UserId = TestAuthUsersProvider.MaxVerstappen.Id,
                UserName = TestAuthUsersProvider.MaxVerstappen.Name,
                MeetingRole = MeetingRole.Member.Name,
                MeetingId = meetingId,
                MeetingName = meetingName,
                MeetingImageId = meetingImageId,
                OccuredOn = joinMeetingDate
            },
            new FeedDTO
            {
                UserId = TestAuthUsersProvider.SergioPerez.Id,
                UserName = TestAuthUsersProvider.SergioPerez.Name,
                MeetingRole = MeetingRole.Owner.Name,
                MeetingId = meetingId,
                MeetingName = meetingName,
                MeetingImageId = meetingImageId,
                OccuredOn = createMeetingDate
            }
        }, options => options.Excluding(x => x.UserImageId));
    }

    [Test]
    public async Task GivenUserWithFollowing_WhenGetFeed_WithLimit_ThenFeedIsReturned()
    {
        // Given
        var userSearchingForFeed = TestAuthUsersProvider.LandoNorris;

        await UsersClient.FollowUser(TestAuthUsersProvider.MaxVerstappen.Id, userSearchingForFeed);
        await UsersClient.FollowUser(TestAuthUsersProvider.SergioPerez.Id, userSearchingForFeed);
        
        var tagName = await GivenOfficialTag(userSearchingForFeed);

        var meetingName = "Eating some Martin's croissants";
        var meetingImageId = Guid.NewGuid();

        var createMeetingDate = new DateTimeOffset(2022, 11, 24, 12, 0, 0, TimeSpan.Zero);
        Clock.Set(createMeetingDate);
        var meetingId = await AddMeeting(meetingName, meetingImageId, tagName, TestAuthUsersProvider.SergioPerez);

        var joinMeetingDate = new DateTimeOffset(2022, 11, 24, 14, 0, 0, TimeSpan.Zero);
        Clock.Set(joinMeetingDate);
        await MeetingsClient.JoinMeeting(meetingId, TestAuthUsersProvider.MaxVerstappen);

        // When
        var feed = (await MeetingsClient.GetFeed(limit: 1, user: userSearchingForFeed)).Content;

        // Then
        feed.NumberOfRecords.Should().Be(1);
        feed.TotalNumberOfRecords.Should().Be(2);
        feed.Records.Should().BeEquivalentTo(new[]
        {
            new FeedDTO
            {
                UserId = TestAuthUsersProvider.MaxVerstappen.Id,
                UserName = TestAuthUsersProvider.MaxVerstappen.Name,
                MeetingRole = MeetingRole.Member.Name,
                MeetingId = meetingId,
                MeetingName = meetingName,
                MeetingImageId = meetingImageId,
                OccuredOn = joinMeetingDate
            },
        }, options => options.Excluding(x => x.UserImageId));
    }
    
    [Test]
    public async Task GivenUserWithFollowing_WhenGetFeed_WithLimit_AndOffset_ThenFeedIsReturned()
    {
        // Given
        var userSearchingForFeed = TestAuthUsersProvider.LandoNorris;

        await UsersClient.FollowUser(TestAuthUsersProvider.MaxVerstappen.Id, userSearchingForFeed);
        await UsersClient.FollowUser(TestAuthUsersProvider.SergioPerez.Id, userSearchingForFeed);
        
        var tagName = await GivenOfficialTag(userSearchingForFeed);

        var meetingName = "Eating some Martin's croissants";
        var meetingImageId = Guid.NewGuid();

        var createMeetingDate = new DateTimeOffset(2022, 11, 24, 12, 0, 0, TimeSpan.Zero);
        Clock.Set(createMeetingDate);
        var meetingId = await AddMeeting(meetingName, meetingImageId, tagName, TestAuthUsersProvider.SergioPerez);

        var joinMeetingDate = new DateTimeOffset(2022, 11, 24, 14, 0, 0, TimeSpan.Zero);
        Clock.Set(joinMeetingDate);
        await MeetingsClient.JoinMeeting(meetingId, TestAuthUsersProvider.MaxVerstappen);

        // When
        var feed = (await MeetingsClient.GetFeed(limit: 1, offset: 1, user: userSearchingForFeed)).Content;

        // Then
        feed.NumberOfRecords.Should().Be(1);
        feed.TotalNumberOfRecords.Should().Be(2);
        feed.Records.Should().BeEquivalentTo(new[]
        {
            new FeedDTO
            {
                UserId = TestAuthUsersProvider.SergioPerez.Id,
                UserName = TestAuthUsersProvider.SergioPerez.Name,
                MeetingRole = MeetingRole.Owner.Name,
                MeetingId = meetingId,
                MeetingName = meetingName,
                MeetingImageId = meetingImageId,
                OccuredOn = createMeetingDate
            },
        }, options => options.Excluding(x => x.UserImageId));
    }

    private async Task<Guid> AddMeeting(string name, Guid imageId, string tag, TestAuthUser user)
    {
        var addMeetingCommand = new AddMeetingCommand(
            name,
            "description",
            10,
            Clock.Now.AddHours(6),
            Clock.Now.AddHours(12), 
            imageId,
            "locationName",
            "locationDescription",
            10,
            10,
            new List<string>{ tag },
            true,
            "no info");
            
        return (await MeetingsClient.AddMeeting(addMeetingCommand, user)).Content!.Id;
    }
    
    private async Task<string> GivenOfficialTag(TestAuthUser user)
    {
        var officialTagName = "officialTagName";
        await TagsClient.AddOfficialTag(new AddOfficialTagCommand(officialTagName), user);
        return officialTagName;
    }
}