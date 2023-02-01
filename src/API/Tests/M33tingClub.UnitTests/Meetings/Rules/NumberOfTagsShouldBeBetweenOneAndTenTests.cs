using System.Collections.Generic;
using FluentAssertions;
using M33tingClub.Domain.Meetings.Rules;
using M33tingClub.Domain.Tags;
using M33tingClub.UnitTests.Utilities;
using NUnit.Framework;

namespace M33tingClub.UnitTests.Meetings.Rules;

public class NumberOfTagsShouldBeBetweenOneAndTenTests : TestBase
{
    [Test]
    [TestCase(1, false)]
    [TestCase(5, false)]
    [TestCase(10, false)]
    [TestCase(0, true)]
    [TestCase(11, true)]
    public void GivenTags_WhenCheckTagsNumberIsBetweenOneAndTen_ThenExpectedValueIsReturned(
        int numberOfTags,
        bool expectedIsBroken)
    {
        // Given
        var listOfTags = new List<TagInfo>();
        for (var i = 0; i < numberOfTags; i++)
        {
            listOfTags.Add(TagInfo.Create(TagName.Create($"Tag_{i}")));
        }
        
        // When
        var isBroken = new NumberOfTagsShouldBeBetweenOneAndTenRule(listOfTags).IsBroken();
        
        // Then
        isBroken.Should().Be(expectedIsBroken);
    }
}