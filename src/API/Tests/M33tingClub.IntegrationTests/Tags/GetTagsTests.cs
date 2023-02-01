using System;
using System.Threading.Tasks;
using FluentAssertions;
using M33tingClub.Application.Tags.AddCommunityTag;
using M33tingClub.Application.Tags.AddOfficialTag;
using M33tingClub.IntegrationTests.Authentication;
using M33tingClub.IntegrationTests.Utilities;
using NUnit.Framework;

namespace M33tingClub.IntegrationTests.Tags;

[TestFixture]
internal class GetTagsTests : TestBase
{
    [Test]
    public async Task GivenMultipleTags_WhenRequestCommunityTags_ReceiveCommunityTags()
    {
        // Given
        var userMax = TestAuthUsersProvider.MaxVerstappen;
        
        await GivenCommunityTags(5, userMax);
        await GivenOfficialTags(4, userMax);
        
        // When
        var tags = (await TagsClient.GetTags(false, userMax)).Content;

        // Then
        tags.Should().HaveCount(5);
        tags.Should().OnlyContain(t => !t.IsOfficial);
    }
    
    [Test]
    public async Task GivenMultipleTags_WhenRequestOfficialTags_ReceiveOfficialTags()
    {
        // Given
        var userMax = TestAuthUsersProvider.MaxVerstappen;
        
        await GivenCommunityTags(5, userMax);
        await GivenOfficialTags(4, userMax);
        
        // When
        var tags = (await TagsClient.GetTags(true, userMax)).Content;

        // Then
        tags.Should().HaveCount(4);
        tags.Should().OnlyContain(t => t.IsOfficial);
    }
    
    [Test]
    public async Task GivenMultipleTags_WhenRequestAllTags_ReceiveAllTags()
    {
        // Given
        var userMax = TestAuthUsersProvider.MaxVerstappen;
        
        await GivenCommunityTags(5, userMax);
        await GivenOfficialTags(4, userMax);
        
        // When
        var tags = (await TagsClient.GetTags(user: userMax)).Content;

        // Then
        tags.Should().HaveCount(9);
        tags.Should().Contain(t => t.IsOfficial);
        tags.Should().Contain(t => !t.IsOfficial);
    }

    private async Task GivenCommunityTags(int numberOfTags, TestAuthUser user)
    {
        for (var i = 0; i < numberOfTags; i++)
        {
            var addCommunityTagCommand = new AddCommunityTagCommand($"Community_{i}");
            await TagsClient.AddCommunityTag(addCommunityTagCommand, user);
        }
    }

    private async Task GivenOfficialTags(int numberOfTags, TestAuthUser user)
    {
        for (var i = 0; i < numberOfTags; i++)
        {
            var addOfficialTagCommand = new AddOfficialTagCommand($"Official_{i}");
            await TagsClient.AddOfficialTag(addOfficialTagCommand, user);
        }
    }
}