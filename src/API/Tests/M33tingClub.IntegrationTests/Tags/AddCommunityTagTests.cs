using System.Threading.Tasks;
using FluentAssertions;
using M33tingClub.Application.Tags;
using M33tingClub.Application.Tags.AddCommunityTag;
using M33tingClub.IntegrationTests.Authentication;
using M33tingClub.IntegrationTests.Utilities;
using NUnit.Framework;

namespace M33tingClub.IntegrationTests.Tags;

[TestFixture]
internal class AddCommunityTagTests : TestBase
{
    //TODO: Add test for adding duplicate tag
    [Test]
    public async Task WhenAddCommunityTag_ThenCommunityTagIsAdded()
    {
        // Given
        var userLando = TestAuthUsersProvider.LandoNorris;
        
        // When
        var tagName = "CommunityTagName";

        var addCommunityTagCommand = new AddCommunityTagCommand(tagName);
        await TagsClient.AddCommunityTag(addCommunityTagCommand, userLando);

        // Then
        var tag = (await TagsClient.GetTag(tagName, userLando)).Content;

        tag.Should().BeEquivalentTo(new TagDTO(tagName, false));
    }
}