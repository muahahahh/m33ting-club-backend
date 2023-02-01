namespace M33tingClub.Application.Users.GetUser;

public class UserDetailsDTO
{
    public Guid Id { get; set; }
    public string FirebaseUid { get; set; }
    public string Name { get; set; }
    public DateTime Birthday { get; set; }
    public string Gender { get; set; }
    public Guid? ImageId { get; set; }
    
    public string PhoneNumber { get; set; }
    public bool IsFollowedByYou { get; set; }
    public bool IsFollowingYou { get; set; }
}