using M33tingClub.Application.Utilities;

namespace M33tingClub.Application.Users.Followers.GetFollowing;

public record GetFollowingQuery(
    Guid UserId,
    int Limit,
    int Offset) : IQuery<PagingInfo<FollowingDTO>>;