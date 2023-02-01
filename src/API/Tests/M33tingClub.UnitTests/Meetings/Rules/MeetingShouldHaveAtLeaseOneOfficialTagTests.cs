using System.Collections.Generic;
using FluentAssertions;
using M33tingClub.Domain.Meetings.Rules;
using M33tingClub.Domain.Tags;
using M33tingClub.UnitTests.Utilities;
using NUnit.Framework;

namespace M33tingClub.UnitTests.Meetings.Rules;

public class MeetingShouldHaveAtLeaseOneOfficialTagTests : TestBase
{
    [Test]
    public void GivenTags_WhenOfficialTags_ThenRuleIsNotBroken()
    {
        // Given
        var numberOfTags = 5;
        var listOfTags = new List<TagInfo>();
        for (var i = 0; i < numberOfTags; i++)
        {
            listOfTags.Add(TagInfo.Create(TagName.Create($"Tag_{i}"), true));
        }
        
        // WHen
        var isBroken = new MeetingShouldHaveAtLeaseOneOfficialTagRule(listOfTags).IsBroken();
        
        // Then
        isBroken.Should().BeFalse();
    }
    
    [Test]
    public void GivenTags_WhenOnlyOneOfficialTags_ThenRuleIsNotBroken()
    {
        // Given
        var numberOfTags = 5;
        var listOfTags = new List<TagInfo>();
        listOfTags.Add(TagInfo.Create(TagName.Create($"OfficialTag"), true));
        for (var i = 0; i < numberOfTags; i++)
        {
            listOfTags.Add(TagInfo.Create(TagName.Create($"Tag_{i}"), false));
        }
        
        // WHen
        var isBroken = new MeetingShouldHaveAtLeaseOneOfficialTagRule(listOfTags).IsBroken();
        
        // Then
        isBroken.Should().BeFalse();
    }
    
    [Test]
    public void GivenTags_WhenNoOfficialTags_ThenRuleIsBroken()
    {
        // Given
        var numberOfTags = 5;
        var listOfTags = new List<TagInfo>();
        for (var i = 0; i < numberOfTags; i++)
        {
            listOfTags.Add(TagInfo.Create(TagName.Create($"Tag_{i}"), false));
        }
        
        // WHen
        var isBroken = new MeetingShouldHaveAtLeaseOneOfficialTagRule(listOfTags).IsBroken();
        
        // Then
        isBroken.Should().BeTrue();
    }
}