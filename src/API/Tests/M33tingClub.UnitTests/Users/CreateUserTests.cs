using System;
using M33tingClub.Domain.Users;
using M33tingClub.Domain.Users.Rules;
using M33tingClub.UnitTests.Utilities;
using NUnit.Framework;

namespace M33tingClub.UnitTests.Users;

[TestFixture]
public class CreateUserTests : TestBase
{
    [Test]
    public void GivenUserData_WithoutName_WhenCreateUser_ThenExceptionIsThrown()
    {
        // Given
        var userId = UserId.CreateNew();
        var firebaseId = "abcdefghijklmnpq";
        var name = string.Empty;
        var birthday = new DateTimeOffset(1990, 12, 20, 0, 0, 0, TimeSpan.Zero).DateTime;
        var gender = UserGender.FromName("Male");
        var phoneNumber = "+48123123123";
        var imageId = Guid.Parse("1b1b7e93-4505-4d58-8b44-d81bfe4412aa");
        
        // When
        var func = () => User.Create(
            userId,
            firebaseId,
            name,
            birthday,
            gender,
            phoneNumber,
            imageId);
        
        // Then
        func.ShouldBrokeRule<UserMustHaveNameRule>();
    }
}