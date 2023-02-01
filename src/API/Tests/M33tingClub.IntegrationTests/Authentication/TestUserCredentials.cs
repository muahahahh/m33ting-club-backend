using System;

namespace M33tingClub.IntegrationTests.Authentication;

internal class TestUserCredentials
{
    public readonly string Email;

    public readonly string Password;
    
    public readonly string PhoneNumber;
    
    public readonly string Name;

    public readonly DateTime Birthday;

    public readonly string Gender;

    public readonly Guid ImageId;

    public TestUserCredentials(
        string email, 
        string password, 
        string phoneNumber,
        string name,
        DateTime birthday, 
        string gender, 
        Guid imageId)
    {
        Email = email;
        Password = password;
        PhoneNumber = phoneNumber;
        Name = name;
        Birthday = birthday;
        Gender = gender;
        ImageId = imageId;
    }
}