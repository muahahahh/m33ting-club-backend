using System;
using System.Threading.Tasks;
using FluentAssertions;
using M33tingClub.Application.Tags;
using M33tingClub.Application.Tags.AddOfficialTag;
using M33tingClub.IntegrationTests.Authentication;
using M33tingClub.IntegrationTests.Utilities;
using NUnit.Framework;

namespace M33tingClub.IntegrationTests.Tags;

[TestFixture]
internal class AddOfficialTagTests : TestBase
{
    //TODO: Add test for adding duplicate tag
    [Test]
    public async Task WhenAddOfficialTag_ThenOfficialTagIsAdded()
    {
        // Given
        var userSergio = TestAuthUsersProvider.SergioPerez;
        
        // When
        var tagName = "OfficialTagName";

        var addOfficialTagCommand = new AddOfficialTagCommand(tagName);
        await TagsClient.AddOfficialTag(addOfficialTagCommand, userSergio);

        // Then
        var tag = (await TagsClient.GetTag(tagName, userSergio)).Content;
        
        tag.Should().BeEquivalentTo(new TagDTO(tagName, true));
    }
}