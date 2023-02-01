using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using M33tingClub.Application.Users.Followers.GetFollowing;
using M33tingClub.Domain.Utilities;
using M33tingClub.IntegrationTests.Authentication;
using M33tingClub.IntegrationTests.Utilities;
using NUnit.Framework;

namespace M33tingClub.IntegrationTests.Users.Followers;

[TestFixture]
internal class FollowUserTests : TestBase
{
    [Test]
    public async Task WhenFollowUser_ThenUserIsFollowed()
    {
        // Given
        var userMax = TestAuthUsersProvider.MaxVerstappen;
        var userPerez = TestAuthUsersProvider.SergioPerez;
        
        // When
        var followDate = new DateTimeOffset(2022, 11, 07, 23, 28, 0, TimeSpan.Zero);
        Clock.Set(followDate);
        await UsersClient.FollowUser(userPerez.Id, userMax);

        // Then
        var pagingInfo = (await UsersClient.GetFollowing(userMax.Id, user: userMax)).Content;

        pagingInfo.TotalNumberOfRecords.Should().Be(1);
        pagingInfo.NumberOfRecords.Should().Be(1);
        pagingInfo.Records.Should().BeEquivalentTo(new[]
        {
            new FollowingDTO
            {
                UserId = userPerez.Id,
                UserName = userPerez.Name,
                UserImageId = userPerez.ImageId,
                FollowingSince = followDate
            }
        });
    }
    
    [Test]
    public async Task WhenFollowUserTryToFollowHimself_ThenErrorIsThrown()
    {
        // Given
        var userMax = TestAuthUsersProvider.MaxVerstappen;

        // When
        var response = await UsersClient.FollowUser(userMax.Id, userMax);

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response.ErrorContent.Errors.Should().HaveCount(1);
        response.ErrorContent.Errors.Should().Contain("User cannot follow himself.");
    }
}