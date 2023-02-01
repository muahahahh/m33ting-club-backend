using System;
using FluentAssertions;
using M33tingClub.Domain.Meetings;
using M33tingClub.Domain.Meetings.Rules;
using M33tingClub.UnitTests.Utilities;
using NUnit.Framework;

namespace M33tingClub.UnitTests.TimeRanges;

[TestFixture]
public class CreateTimeRangeTests : TestBase
{
	[Test]
	public void WhenCreateTimeRange_WithValidDates_ThenIsCreated()
	{
		// When
		var startDate = new DateTimeOffset(2022, 5, 25, 12, 00, 00, TimeSpan.Zero);
		var endDate = new DateTimeOffset(2022, 5, 25, 13, 00, 00, TimeSpan.Zero);

		var timeRange = TimeRange.Create(startDate, endDate);

		// Then
		timeRange.Start.Should().Be(startDate);
		timeRange.End.Should().Be(endDate);
	}

	[Test]
	public void WhenCreateTimeRange_AndStartIsEqualEnd_ExceptionIsThrown()
	{
		// When
		var startDate = new DateTimeOffset(2022, 5, 25, 12, 00, 00, TimeSpan.Zero);
		var endDate = new DateTimeOffset(2022, 5, 25, 12, 00, 00, TimeSpan.Zero);

		var func = () => TimeRange.Create(startDate, endDate);

		// Then
		func.ShouldBrokeRule<StartDateMustBeLessThanEndDateRule>();
	}
	
	[Test]
	public void WhenCreateTimeRange_AndStartIsGreaterThenEnd_ExceptionIsThrown()
	{
		// When
		var startDate = new DateTimeOffset(2022, 5, 25, 12, 00, 00, TimeSpan.Zero);
		var endDate = new DateTimeOffset(2022, 5, 25, 11, 00, 00, TimeSpan.Zero);

		var func = () => TimeRange.Create(startDate, endDate);

		// Then
		func.ShouldBrokeRule<StartDateMustBeLessThanEndDateRule>();
	}
}