using System;
using System.Threading.Tasks;
using FluentAssertions;
using M33tingClub.Application.Users.Followers.GetFollowers;
using M33tingClub.Domain.Utilities;
using M33tingClub.IntegrationTests.Authentication;
using M33tingClub.IntegrationTests.Utilities;
using NUnit.Framework;

namespace M33tingClub.IntegrationTests.Users.Followers;

[TestFixture]
internal class GetFollowersTests : TestBase
{
    [Test]
    public async Task GivenUsersFollowingMaxVerstappen_WhenGetFollowers_ThenExpectedRecordsAreReturned()
    {
        // Given
        var userMax = TestAuthUsersProvider.MaxVerstappen;
        var userPerez = TestAuthUsersProvider.SergioPerez;
        var userLando = TestAuthUsersProvider.LandoNorris;
        
        var firstFollowDate = new DateTimeOffset(2022, 11, 07, 23, 28, 0, TimeSpan.Zero);
        Clock.Set(firstFollowDate);
        await UsersClient.FollowUser(userMax.Id, userPerez);

        var secondFollowDate = new DateTimeOffset(2022, 11, 08, 23, 28, 0, TimeSpan.Zero);
        Clock.Set(secondFollowDate);
        await UsersClient.FollowUser(userMax.Id, userLando);
        
        // When
        var pagingInfo = (await UsersClient.GetFollowers(userMax.Id, offset: 0, limit: 2, user: userPerez)).Content;
        
        // Then
        pagingInfo.TotalNumberOfRecords.Should().Be(2);
        pagingInfo.NumberOfRecords.Should().Be(2);
        pagingInfo.Records.Should().BeEquivalentTo(new[]
        {
            new FollowerDTO
            {
                UserId = userPerez.Id,
                UserName = userPerez.Name,
                UserImageId = userPerez.ImageId,
                FollowerSince = firstFollowDate
            },
            new FollowerDTO
            {
                UserId = userLando.Id,
                UserName = userLando.Name,
                UserImageId = userLando.ImageId,
                FollowerSince = secondFollowDate
            }
        });
    }
    
    [Test]
    public async Task GivenUsersFollowingMaxVerstappen_WhenGetFollowers_AndLimitIsOne_LastFollowedIsReturned()
    {
        // Given
        var userMax = TestAuthUsersProvider.MaxVerstappen;
        var userPerez = TestAuthUsersProvider.SergioPerez;
        var userLando = TestAuthUsersProvider.LandoNorris;
        
        var firstFollowDate = new DateTimeOffset(2022, 11, 07, 23, 28, 0, TimeSpan.Zero);
        Clock.Set(firstFollowDate);
        await UsersClient.FollowUser(userMax.Id, userPerez);

        var secondFollowDate = new DateTimeOffset(2022, 11, 08, 23, 28, 0, TimeSpan.Zero);
        Clock.Set(secondFollowDate);
        await UsersClient.FollowUser(userMax.Id, userLando);
        
        // When
        var pagingInfo = (await UsersClient.GetFollowers(userMax.Id, offset: 0, limit: 1, user: userLando)).Content;
        
        // Then
        pagingInfo.TotalNumberOfRecords.Should().Be(2);
        pagingInfo.NumberOfRecords.Should().Be(1);
        pagingInfo.Records.Should().BeEquivalentTo(new[]
        {
            new FollowerDTO
            {
                UserId = userLando.Id,
                UserName = userLando.Name,
                UserImageId = userLando.ImageId,
                FollowerSince = secondFollowDate
            }
        });
    }
}