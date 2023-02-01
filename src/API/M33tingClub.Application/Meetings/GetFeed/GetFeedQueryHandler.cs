using System.Data;
using Dapper;
using M33tingClub.Application.Users.Followers;
using M33tingClub.Application.Utilities;
using M33tingClub.Domain.Auth;

namespace M33tingClub.Application.Meetings.GetFeed;

public class GetFeedQueryHandler : IQueryHandler<GetFeedQuery, PagingInfo<FeedDTO>>
{
    private readonly IDbConnection _connection;
    private readonly IUserContext _userContext;
    
    public GetFeedQueryHandler(
        ISqlConnectionFactory sqlConnectionFactory, 
        IUserContext userContext)
    {
        _connection = sqlConnectionFactory.GetOpenConnection();
        _userContext = userContext;
    }
    
    public async Task<PagingInfo<FeedDTO>> Handle(GetFeedQuery query, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        
        var followingIds = 
            (await FollowershipSupplier.GetFollowing(_connection, userId))
            .Select(x => x.UserId).ToList();
        
        var feed = await GetFeed(followingIds, query.Limit, query.Offset);

        var totalNumberOfFeed = await GetTotalNumberOfFeed(followingIds);

        return new PagingInfo<FeedDTO>(feed, totalNumberOfFeed);
    }

    private async Task<List<FeedDTO>> GetFeed(List<Guid> userIds, int limit, int offset)
    {
        var sql = "select " +
                  $"\"user_id\" as {nameof(FeedDTO.UserId)}," + 
                  $"\"user_name\" as {nameof(FeedDTO.UserName)}, " +
                  $"\"user_image_id\" as {nameof(FeedDTO.UserImageId)}, " +
                  $"\"meeting_role\" as {nameof(FeedDTO.MeetingRole)}, " +
                  $"\"meeting_id\" as {nameof(FeedDTO.MeetingId)}, " +
                  $"\"meeting_name\" as {nameof(FeedDTO.MeetingName)}, " +
                  $"\"meeting_image_id\" as {nameof(FeedDTO.MeetingImageId)}, " +
                  $"\"occured_on\" as {nameof(FeedDTO.OccuredOn)} " +

                  "FROM \"app\".\"v_feed\" " +
                  "/**where**/" +
                  "/**orderby**/";
        
        var sqlBuilder = new SqlBuilder();
        var sqlTemplate = sqlBuilder.AddTemplate(sql);

        sqlBuilder.Where("\"user_id\" = ANY(@UserIds)");
        
        sqlBuilder.OrderBy("occured_on desc");
        
        sqlTemplate = sqlBuilder.AddTemplate($"{sqlTemplate.RawSql} LIMIT @Limit OFFSET @Offset");

        return (await _connection.QueryAsync<FeedDTO>(
            sqlTemplate.RawSql,
            new
            {
                UserIds = userIds,
                Limit = limit,
                Offset = offset,
            })).ToList();
    }

    private async Task<int> GetTotalNumberOfFeed(List<Guid> userIds)
    {
        var sql = "select count(*) " +
                  "FROM \"app\".\"v_feed\" " +
                  "/**where**/";
        
        var sqlBuilder = new SqlBuilder();
        var sqlTemplate = sqlBuilder.AddTemplate(sql);
        
        sqlBuilder.Where("\"user_id\" = ANY(@UserIds)");

        return await _connection.ExecuteScalarAsync<int>(
            sqlTemplate.RawSql,
            new
            {
                UserIds = userIds,
            });
    }
}