using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using M33tingClub.Application.Tags.AddCommunityTag;
using M33tingClub.Application.Tags.AddOfficialTag;
using M33tingClub.IntegrationTests.Authentication;
using M33tingClub.IntegrationTests.Utilities;
using NUnit.Framework;

namespace M33tingClub.IntegrationTests.Tags;

[TestFixture]
internal class SearchTagsTests : TestBase
{
    [Test]
    public async Task GivenTags_WhenSearchByPhrase_ThenExpectedTagsAreReturnedInCorrectOrder()
    {
        // Given
        var userMax = TestAuthUsersProvider.MaxVerstappen;

        await GivenCommunityTag("community1", userMax);
        await GivenOfficialTag("official1", userMax);
        await GivenCommunityTag("community2", userMax);
        await GivenOfficialTag("official2", userMax);
        
        // When
        var pagingInfo = (await TagsClient.SearchTags(
            "",
            2,
            0,
            userMax)).Content;

        var tagNames = pagingInfo.Records.Select(x => x.Name).ToList();

        // Then
        pagingInfo.Should().NotBeNull();
        pagingInfo.NumberOfRecords.Should().Be(2);
        pagingInfo.TotalNumberOfRecords.Should().Be(4);
        tagNames.Should().BeEquivalentTo(new[]
        {
            "official1",
            "official2"
        });
    }
    
    [Test]
    public async Task GivenTags_WhenSearchByPhrase_Ort_ThenExpectedTagsAreReturned()
    {
        // Given
        var userMax = TestAuthUsersProvider.MaxVerstappen;

        await GivenCommunityTag("szkola", userMax);
        await GivenCommunityTag("SPort", userMax);
        await GivenCommunityTag("Ortografia", userMax);
        await GivenCommunityTag("koszykowka", userMax);

        // When
        var pagingInfo = (await TagsClient.SearchTags(
            "ort",
            user: userMax)).Content;

        var tagNames = pagingInfo.Records.Select(x => x.Name).ToList();

        // Then
        pagingInfo.Should().NotBeNull();
        pagingInfo.NumberOfRecords.Should().Be(2);
        pagingInfo.TotalNumberOfRecords.Should().Be(2);
        tagNames.Should().BeEquivalentTo(new[]
        {
            "SPort",
            "Ortografia"
        });
    }
    
    [Test]
    public async Task GivenTags_WhenSearchByPhrase_Sp_ThenExpectedTagsAreReturned()
    {
        // Given
        var userMax = TestAuthUsersProvider.MaxVerstappen;

        await GivenOfficialTag("szkola", userMax);
        await GivenOfficialTag("sport", userMax);
        await GivenOfficialTag("Spadanie", userMax);
        await GivenOfficialTag("SPANKO", userMax);
        await GivenOfficialTag("JEDZENIE", userMax);

        // When
        var pagingInfo = (await TagsClient.SearchTags(
            "SP",
            user: userMax)).Content;

        var tagNames = pagingInfo.Records.Select(x => x.Name).ToList();

        // Then
        pagingInfo.Should().NotBeNull();
        pagingInfo.NumberOfRecords.Should().Be(3);
        pagingInfo.TotalNumberOfRecords.Should().Be(3);
        tagNames.Should().BeEquivalentTo(new[]
        {
            "sport",
            "Spadanie",
            "SPANKO"
        });
    }
    
    private async Task GivenCommunityTag(string name, TestAuthUser user)
    {
        var addCommunityTagCommand = new AddCommunityTagCommand(name);
        await TagsClient.AddCommunityTag(addCommunityTagCommand, user);
    }
    
    private async Task GivenOfficialTag(string name, TestAuthUser user)
    {
        var addOfficialTagCommand = new AddOfficialTagCommand(name);
        await TagsClient.AddOfficialTag(addOfficialTagCommand, user);
    }
}