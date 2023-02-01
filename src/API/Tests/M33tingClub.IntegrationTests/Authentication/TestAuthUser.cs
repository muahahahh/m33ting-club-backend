using System;

namespace M33tingClub.IntegrationTests.Authentication;

internal class TestAuthUser
{
    public readonly Guid Id;

    public readonly string FirebaseUid;

    public readonly string Name;

    public readonly string PhoneNumber;

    public readonly DateTime Birthday;

    public readonly string Gender;

    public readonly Guid ImageId;
    
    public readonly string AuthToken;

    public TestAuthUser(
        Guid id, 
        string firebaseUid, 
        string name,
        string phoneNumber,
        DateTime birthday, 
        string gender, 
        Guid imageId,
        string authToken)
    {
        Id = id;
        FirebaseUid = firebaseUid;
        Name = name;
        PhoneNumber = phoneNumber;
        Birthday = birthday;
        Gender = gender;
        ImageId = imageId;
        AuthToken = authToken;
    }
}