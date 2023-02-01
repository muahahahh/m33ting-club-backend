using System.Threading.Tasks;
using FluentAssertions;
using M33tingClub.IntegrationTests.Authentication;
using M33tingClub.IntegrationTests.Utilities;
using NUnit.Framework;

namespace M33tingClub.IntegrationTests.Users.Followers;

[TestFixture]
internal class UnfollowUserTests : TestBase
{
    [Test]
    public async Task GivenFollowedUser_WhenUnfollowUser_ThenUserIsNoLongerFollowed()
    {
        // Given
        var userMax = TestAuthUsersProvider.MaxVerstappen;
        var userPerez = TestAuthUsersProvider.SergioPerez;
        
        await UsersClient.FollowUser(userPerez.Id, userMax);
        
        // When
        await UsersClient.UnfollowUser(userPerez.Id, userMax);

        // Then
        var pagingInfo = (await UsersClient.GetFollowing(userMax.Id, user: userMax)).Content;

        pagingInfo.TotalNumberOfRecords.Should().Be(0);
        pagingInfo.NumberOfRecords.Should().Be(0);
        pagingInfo.Records.Should().BeEmpty();
    }
}