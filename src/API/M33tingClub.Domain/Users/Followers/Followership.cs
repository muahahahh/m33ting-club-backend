using M33tingClub.Domain.Users.Followers.Rules;
using M33tingClub.Domain.Utilities;

namespace M33tingClub.Domain.Users.Followers;

public class Followership : Entity
{
    public UserId FollowerId { get; private set; }
    
    public UserId FollowingId { get; private set; }

    private DateTimeOffset _createdAt;
    
    private Followership()
    {
        // For EF core
    }

    private Followership(
        UserId followerId, 
        UserId followingId, 
        DateTimeOffset createdAt)
    {
        CheckRule(new UserCannotFollowHimselfRule(followerId, followingId));
        
        FollowerId = followerId;
        FollowingId = followingId;
        _createdAt = createdAt;
    }

    public static Followership Create(
        UserId followerId,
        UserId followingId,
        DateTimeOffset currentData)
    {
        return new(
            followerId,
            followingId,
            currentData);
    }
}