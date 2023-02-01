using System;
using System.Threading.Tasks;
using FluentAssertions;
using M33tingClub.Application.Users.Followers.GetFollowing;
using M33tingClub.Domain.Utilities;
using M33tingClub.IntegrationTests.Authentication;
using M33tingClub.IntegrationTests.Utilities;
using NUnit.Framework;

namespace M33tingClub.IntegrationTests.Users.Followers;

[TestFixture]
internal class GetFollowingTests : TestBase
{
    [Test]
    public async Task GivenUsersFollowingByMaxVerstappen_WhenGetFollowing_ThenExpectedRecordsAreReturned()
    {
        // Given
        var userMax = TestAuthUsersProvider.MaxVerstappen;
        var userPerez = TestAuthUsersProvider.SergioPerez;
        var userLando = TestAuthUsersProvider.LandoNorris;
        
        var firstFollowDate = new DateTimeOffset(2022, 11, 07, 23, 28, 0, TimeSpan.Zero);
        Clock.Set(firstFollowDate);
        await UsersClient.FollowUser(userPerez.Id, userMax);

        var secondFollowDate = new DateTimeOffset(2022, 11, 08, 23, 28, 0, TimeSpan.Zero);
        Clock.Set(secondFollowDate);
        await UsersClient.FollowUser(userLando.Id, userMax);
        
        // When
        var pagingInfo = (await UsersClient.GetFollowing(userMax.Id, offset: 0, limit: 2, user: userMax)).Content;
        
        // Then
        pagingInfo.TotalNumberOfRecords.Should().Be(2);
        pagingInfo.NumberOfRecords.Should().Be(2);
        pagingInfo.Records.Should().BeEquivalentTo(new[]
        {
            new FollowingDTO
            {
                UserId = userPerez.Id,
                UserName = userPerez.Name,
                UserImageId = userPerez.ImageId,
                FollowingSince = firstFollowDate
            },
            new FollowingDTO
            {
                UserId = userLando.Id,
                UserName = userLando.Name,
                UserImageId = userLando.ImageId,
                FollowingSince = secondFollowDate
            }
        });
    }
    
    [Test]
    public async Task GivenUsersFollowingByMAxVerstappen_WhenGetFollowing_AndLimitIsOne_LastFollowedIsReturned()
    {
        // Given
        var userMax = TestAuthUsersProvider.MaxVerstappen;
        var userPerez = TestAuthUsersProvider.SergioPerez;
        var userLando = TestAuthUsersProvider.LandoNorris;
        
        var firstFollowDate = new DateTimeOffset(2022, 11, 07, 23, 28, 0, TimeSpan.Zero);
        Clock.Set(firstFollowDate);
        await UsersClient.FollowUser(userPerez.Id, userMax);

        var secondFollowDate = new DateTimeOffset(2022, 11, 08, 23, 28, 0, TimeSpan.Zero);
        Clock.Set(secondFollowDate);
        await UsersClient.FollowUser(userLando.Id, userMax);
        
        // When
        var pagingInfo = (await UsersClient.GetFollowing(userMax.Id, offset: 0, limit: 1, user: userMax)).Content;
        
        // Then
        pagingInfo.TotalNumberOfRecords.Should().Be(2);
        pagingInfo.NumberOfRecords.Should().Be(1);
        pagingInfo.Records.Should().BeEquivalentTo(new[]
        {
            new FollowingDTO
            {
                UserId = userLando.Id,
                UserName = userLando.Name,
                UserImageId = userLando.ImageId,
                FollowingSince = secondFollowDate
            }
        });
    }
}