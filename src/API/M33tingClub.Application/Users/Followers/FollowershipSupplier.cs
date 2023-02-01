using System.Data;
using Dapper;
using M33tingClub.Application.Users.Followers.GetFollowers;
using M33tingClub.Application.Users.Followers.GetFollowing;

namespace M33tingClub.Application.Users.Followers;

public class FollowershipSupplier
{
    internal static async Task<List<FollowerDTO>> GetFollowers(
        IDbConnection connection,
        Guid followingId,
        int limit,
        int offset)
    {
        var sql = "select " +
                  $"\"follower_id\" as {nameof(FollowerDTO.UserId)}," + 
                  $"\"follower_name\" as {nameof(FollowerDTO.UserName)}, " +
                  $"\"follower_image_id\" as {nameof(FollowerDTO.UserImageId)}, " +
                  $"\"created_at\" as {nameof(FollowerDTO.FollowerSince)} " +

                  "FROM \"app\".\"v_followership\" " +
                  "/**where**/" +
                  "/**orderby**/";
        
        var sqlBuilder = new SqlBuilder();
        var sqlTemplate = sqlBuilder.AddTemplate(sql);

        sqlBuilder.Where("\"following_id\" = @FollowingId");
        
        sqlBuilder.OrderBy(
            "\"created_at\" desc, follower_id");
        
        sqlTemplate = sqlBuilder.AddTemplate($"{sqlTemplate.RawSql} LIMIT @Limit OFFSET @Offset");

        return (await connection.QueryAsync<FollowerDTO>(
            sqlTemplate.RawSql,
            new
        {
            FollowingId = followingId,
            Limit = limit,
            Offset = offset
        })).ToList();
    }

    internal static async Task<int> GetTotalNumberOfFollowers(IDbConnection connection,
        Guid followingId)
    {
        var sql = "select count(*)" +
                  "FROM \"app\".\"v_followership\" " +
                  "/**where**/";
        
        var sqlBuilder = new SqlBuilder();
        var sqlTemplate = sqlBuilder.AddTemplate(sql);
        
        sqlBuilder.Where("\"following_id\" = @FollowingId");
        
        return await connection.ExecuteScalarAsync<int>(
            sqlTemplate.RawSql,
            new
            {
                FollowingId = followingId,
            });
    }
    
    internal static async Task<List<FollowingDTO>> GetFollowing(
        IDbConnection connection,
        Guid followerId,
        int? limit = null,
        int? offset = null)
    {
        var sql = "select " +
                  $"\"following_id\" as {nameof(FollowingDTO.UserId)}," + 
                  $"\"following_name\" as {nameof(FollowingDTO.UserName)}, " +
                  $"\"following_image_id\" as {nameof(FollowingDTO.UserImageId)}, " +
                  $"\"created_at\" as {nameof(FollowingDTO.FollowingSince)} " +

                  "FROM \"app\".\"v_followership\" " +
                  "/**where**/" +
                  "/**orderby**/";
        
        var sqlBuilder = new SqlBuilder();
        var sqlTemplate = sqlBuilder.AddTemplate(sql);

        sqlBuilder.Where("\"follower_id\" = @FollowerId");
        
        sqlBuilder.OrderBy(
            "\"created_at\" desc");

        if (limit is not null && offset is not null)
        {
            sqlTemplate = sqlBuilder.AddTemplate($"{sqlTemplate.RawSql} LIMIT @Limit OFFSET @Offset");   
        }

        return (await connection.QueryAsync<FollowingDTO>(
            sqlTemplate.RawSql,
            new
            {
                FollowerId = followerId,
                Limit = limit,
                Offset = offset
            })).ToList();
    }

    internal static async Task<int> GetTotalNumberOfFollowing(IDbConnection connection,
        Guid followerId)
    {
        var sql = "select count(*)" +
                  "FROM \"app\".\"v_followership\" " +
                  "/**where**/";
        
        var sqlBuilder = new SqlBuilder();
        var sqlTemplate = sqlBuilder.AddTemplate(sql);
        
        sqlBuilder.Where("\"follower_id\" = @FollowerId");
        
        return await connection.ExecuteScalarAsync<int>(
            sqlTemplate.RawSql,
            new
            {
                FollowerId = followerId,
            });
    }
}