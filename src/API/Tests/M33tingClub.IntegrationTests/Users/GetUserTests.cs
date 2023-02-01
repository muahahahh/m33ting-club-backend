using System.Threading.Tasks;
using FluentAssertions;
using M33tingClub.Application.Users.GetUser;
using M33tingClub.IntegrationTests.Authentication;
using M33tingClub.IntegrationTests.Utilities;
using NUnit.Framework;

namespace M33tingClub.IntegrationTests.Users;

[TestFixture]
internal class GetUserTests : TestBase
{
    [Test]
    public async Task GivenNoFollowers_WhenGetUser_ThenExpectedResultIsReturned()
    {
        // Given
        var currentUser = TestAuthUsersProvider.SergioPerez;
        var searchedUser = TestAuthUsersProvider.LandoNorris;
        
        // When
        var user = (await UsersClient.GetUser(
            currentUser.AuthToken,
            searchedUser.Id)).Content;

        // Then
        user.Should().BeEquivalentTo(new UserDetailsDTO
        {
            Id = searchedUser.Id,
            FirebaseUid = searchedUser.FirebaseUid,
            Name = searchedUser.Name,
            Birthday = searchedUser.Birthday,
            Gender = searchedUser.Gender,
            PhoneNumber = searchedUser.PhoneNumber,
            IsFollowedByYou = false,
            IsFollowingYou = false
        }, options => options.Excluding(x => x.ImageId));
    }
    
    [Test]
    public async Task GivenIFollowUser_WhenGetUser_ThenExpectedResultIsReturned()
    {
        // Given
        var currentUser = TestAuthUsersProvider.SergioPerez;
        var searchedUser = TestAuthUsersProvider.LandoNorris;
        
        await UsersClient.FollowUser(searchedUser.Id, currentUser);
        
        // When
        var user = (await UsersClient.GetUser(
            currentUser.AuthToken,
            searchedUser.Id)).Content;

        // Then
        user.Should().BeEquivalentTo(new UserDetailsDTO
        {
            Id = searchedUser.Id,
            FirebaseUid = searchedUser.FirebaseUid,
            Name = searchedUser.Name,
            Birthday = searchedUser.Birthday,
            Gender = searchedUser.Gender,
            PhoneNumber = searchedUser.PhoneNumber,
            IsFollowedByYou = true,
            IsFollowingYou = false
        }, options => options.Excluding(x => x.ImageId));
    }
    
    [Test]
    public async Task UserFollowsMe_WhenGetUser_ThenExpectedResultIsReturned()
    {
        // Given
        var currentUser = TestAuthUsersProvider.SergioPerez;
        var searchedUser = TestAuthUsersProvider.LandoNorris;
        
        await UsersClient.FollowUser(currentUser.Id, searchedUser);
        
        // When
        var user = (await UsersClient.GetUser(
            currentUser.AuthToken,
            searchedUser.Id)).Content;

        // Then
        user.Should().BeEquivalentTo(new UserDetailsDTO
        {
            Id = searchedUser.Id,
            FirebaseUid = searchedUser.FirebaseUid,
            Name = searchedUser.Name,
            Birthday = searchedUser.Birthday,
            Gender = searchedUser.Gender,
            PhoneNumber = searchedUser.PhoneNumber,
            IsFollowedByYou = false,
            IsFollowingYou = true
        }, options => options.Excluding(x => x.ImageId));
    }

    [Test]
    public async Task GivenUsersFollowEachOther_WhenGetUser_ThenExpectedResultIsReturned()
    {
        // Given
        var currentUser = TestAuthUsersProvider.SergioPerez;
        var searchedUser = TestAuthUsersProvider.LandoNorris;
        
        await UsersClient.FollowUser(searchedUser.Id, currentUser);
        await UsersClient.FollowUser(currentUser.Id, searchedUser);
        
        // When
        var user = (await UsersClient.GetUser(
            currentUser.AuthToken,
            searchedUser.Id)).Content;

        // Then
        user.Should().BeEquivalentTo(new UserDetailsDTO
        {
            Id = searchedUser.Id,
            FirebaseUid = searchedUser.FirebaseUid,
            Name = searchedUser.Name,
            Birthday = searchedUser.Birthday,
            Gender = searchedUser.Gender,
            PhoneNumber = searchedUser.PhoneNumber,
            IsFollowedByYou = true,
            IsFollowingYou = true
        }, options => options.Excluding(x => x.ImageId));
    }
}