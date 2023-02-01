using System.Data;
using M33tingClub.Application.Utilities;

namespace M33tingClub.Application.Users.Followers.GetFollowing;

public class GetFollowingQueryHandler : IQueryHandler<GetFollowingQuery, PagingInfo<FollowingDTO>>
{
    private readonly IDbConnection _connection;

    public GetFollowingQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _connection = sqlConnectionFactory.GetOpenConnection();
    }
    
    public async Task<PagingInfo<FollowingDTO>> Handle(GetFollowingQuery query, CancellationToken cancellationToken)
    {
        var following = await FollowershipSupplier.GetFollowing(
            _connection,
            query.UserId,
            query.Limit,
            query.Offset);

        var totalNumberOfFollowing = await FollowershipSupplier.GetTotalNumberOfFollowing(
            _connection,
            query.UserId);

        return new PagingInfo<FollowingDTO>(following, totalNumberOfFollowing);
    }
}