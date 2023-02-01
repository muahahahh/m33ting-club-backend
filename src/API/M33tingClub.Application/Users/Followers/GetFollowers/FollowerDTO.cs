namespace M33tingClub.Application.Users.Followers.GetFollowers;

public class FollowerDTO
{
    public Guid UserId { get; set; }

    public string UserName { get; set; } = string.Empty;
    
    public Guid UserImageId { get; set; }

    public DateTimeOffset FollowerSince { get; set; }
}