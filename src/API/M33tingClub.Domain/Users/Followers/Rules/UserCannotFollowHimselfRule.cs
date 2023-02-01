using M33tingClub.Domain.Utilities;

namespace M33tingClub.Domain.Users.Followers.Rules;

public class UserCannotFollowHimselfRule : IRule
{
    private readonly UserId _followerId;

    private readonly UserId _followingId;

    public UserCannotFollowHimselfRule(
        UserId followerId, 
        UserId followingId)
    {
        _followerId = followerId;
        _followingId = followingId;
    }

    public bool IsBroken() => _followerId.Value == _followingId.Value;

    public string Message => "User cannot follow himself.";
    
    public RuleExceptionKind Kind => RuleExceptionKind.BadRequest;
}