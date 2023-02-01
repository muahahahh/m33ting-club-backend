using M33tingClub.Application.Utilities;

namespace M33tingClub.Application.Users.Followers.GetFollowers;

public record GetFollowersQuery(
    Guid UserId,
    int Limit,
    int Offset) : IQuery<PagingInfo<FollowerDTO>>;