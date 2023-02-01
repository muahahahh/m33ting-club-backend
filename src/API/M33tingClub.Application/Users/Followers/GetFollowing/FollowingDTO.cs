namespace M33tingClub.Application.Users.Followers.GetFollowing;

public class FollowingDTO
{
    public Guid UserId { get; set; }

    public string UserName { get; set; } = string.Empty;

    public Guid UserImageId { get; set; }

    public DateTimeOffset FollowingSince { get; set; }
}