namespace M33tingClub.Application.Users.GetUserSelf;

public class UserDTO
{
    public Guid Id { get; set; }
    
    public string FirebaseUid { get; set; }
    
    public string Name { get; set; }
    
    public DateTime Birthday { get; set; }
    
    public string Gender { get; set; }
    
    public Guid? ImageId { get; set; }
    
    public string PhoneNumber { get; set; }
}