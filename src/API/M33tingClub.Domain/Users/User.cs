using M33tingClub.Domain.Users.Rules;
using M33tingClub.Domain.Utilities;

namespace M33tingClub.Domain.Users;

public class User : Entity
{
    public UserId Id { get; private set; }

    public string FirebaseId { get; private set; }

    public bool IsDeleted { get; private set; }
    
    private string _name;
    
    private DateTime _birthday;
    
    private UserGender _gender;
    
    private string _phoneNumber;
    
    private Guid? _imageId;

    private User()
    {
        // For EF core
    }

    private User(
        UserId id, 
        string firebaseId, 
        string name, 
        DateTime birthday, 
        UserGender gender, 
        string phoneNumber,
        Guid? imageId = null)
    {
        CheckRule(new UserMustHaveNameRule(name));

        Id = id;
        FirebaseId = firebaseId;
        _name = name;
        _birthday = birthday;
        _gender = gender;
        _imageId = imageId;
        _phoneNumber = phoneNumber;
        IsDeleted = false;
    }
    
    public static User Create(
        UserId id, 
        string firebaseId, 
        string name,
        DateTime birthday, 
        UserGender gender, 
        string phoneNumber,
        Guid? imageId = null) 
        => new(
            id, 
            firebaseId, 
            name, 
            birthday, 
            gender, 
            phoneNumber,
            imageId);

    public void MarkAsDeleted()
    {
        FirebaseId = "Deleted";
        _name = "Deleted";
        _birthday = new DateTime(1410, 7, 15, 0, 0, 0, DateTimeKind.Utc);
        _gender = UserGender.Other;
        _phoneNumber = "Deleted";
        _imageId = null;
        IsDeleted = true;
    }
}
