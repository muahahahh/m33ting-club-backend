using System.Data;
using M33tingClub.Application.Utilities;

namespace M33tingClub.Application.Users.Followers.GetFollowers;

public class GetFollowersQueryHandler : IQueryHandler<GetFollowersQuery, PagingInfo<FollowerDTO>>
{
    private readonly IDbConnection _connection;

    public GetFollowersQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _connection = sqlConnectionFactory.GetOpenConnection();
    }
    
    public async Task<PagingInfo<FollowerDTO>> Handle(GetFollowersQuery query, CancellationToken cancellationToken)
    {
        var followers = await FollowershipSupplier.GetFollowers(
            _connection,
            query.UserId,
            query.Limit,
            query.Offset);

        var totalNumberOfFollowers = await FollowershipSupplier.GetTotalNumberOfFollowers(
            _connection,
            query.UserId);

        return new PagingInfo<FollowerDTO>(followers, totalNumberOfFollowers);
    }
}