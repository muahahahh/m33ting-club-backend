using System.Collections.Generic;
using M33tingClub.Domain.Meetings;
using M33tingClub.Domain.Meetings.Rules;
using M33tingClub.UnitTests.Utilities;
using NUnit.Framework;

namespace M33tingClub.UnitTests.Meetings.Rules;

[TestFixture]
public class OnlyPendingApplicationCanBeRejectedRuleTests : TestBase
{
    private static IEnumerable<TestCaseData> _testCases
    {
        get
        {
            yield return new TestCaseData(MeetingApplicationStatus.Pending).Returns(false);
            yield return new TestCaseData(MeetingApplicationStatus.Accepted).Returns(true);
            yield return new TestCaseData(MeetingApplicationStatus.Rejected).Returns(true);
        }
    }

    [Test]
    [TestCaseSource(nameof(_testCases))]
    public bool GivenApplicationStatus_WhenCheckOnlyPendingApplicationCanBeRejectedRule_TenExpectedResultIsReturned(
        MeetingApplicationStatus applicationStatus)
        => new OnlyPendingApplicationCanBeRejectedRule(applicationStatus).IsBroken();
}