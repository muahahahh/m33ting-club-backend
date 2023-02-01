using System.Data;
using Dapper;
using M33tingClub.Domain.Meetings;
using M33tingClub.Domain.Utilities.Exceptions;

namespace M33tingClub.Application.Meetings;

internal static class MeetingsSupplier
{
    private static (SqlBuilder sqlBuilder, SqlBuilder.Template sqlTemplate) CreateBaseQuery()
    {
        var sql = "select " +
                  $"\"id\" as {nameof(MeetingDTO.Id)}," + 
                  $"\"name\" as {nameof(MeetingDTO.Name)}, " +
                  $"\"description\" as {nameof(MeetingDTO.Description)}, " +
                  $"\"participants_limit\" as {nameof(MeetingDTO.ParticipantsLimit)}, " +
                  $"\"start_date\" as {nameof(MeetingDTO.StartDate)}, " +
                  $"\"end_date\" as {nameof(MeetingDTO.EndDate)}, " +
                  $"\"image_id\" as {nameof(MeetingDTO.ImageId)}, " +
                  $"\"location_name\" as {nameof(MeetingDTO.LocationName)}, " +
                  $"\"location_description\" as {nameof(MeetingDTO.LocationDescription)}, " +
                  $"\"location_coordinates\"[0] as {nameof(MeetingDTO.Longitude)}, " +
                  $"\"location_coordinates\"[1] as {nameof(MeetingDTO.Latitude)}, " +
                  $"round((\"app\".\"calculate_distance_m\"(\"location_coordinates\", point(@Longitude, @Latitude)) / 1000)::numeric, 1) as {nameof(MeetingDTO.Distance)}, " +
                  $"\"tag_names\" as {nameof(MeetingDTO.Tags)}, " +
                  $"\"participants\" as {nameof(MeetingDTO.Participants)}, " +
                  $"\"status\" as {nameof(MeetingDTO.Status)}, " +
                  $"\"is_public\" as {nameof(MeetingDTO.IsPublic)}, " +
                  $"\"confidential_info\" as {nameof(MeetingDTO.ConfidentialInfo)} " +

                  "FROM \"app\".\"v_meeting\" " +
                  "/**innerjoin**/ " + 
                  "/**where**/" +
                  "/**orderby**/";

        var sqlBuilder = new SqlBuilder();
        var sqlTemplate = sqlBuilder.AddTemplate(sql);

        return (sqlBuilder, sqlTemplate);
    }
    
    private static (SqlBuilder sqlBuilder, SqlBuilder.Template sqlTemplate) CreateCountBaseQuery()
    {
        var sql = "select count(*) " +
                  "FROM \"app\".\"v_meeting\" " +
                  "/**innerjoin**/ " + 
                  "/**where**/" +
                  "/**orderby**/";

        var sqlBuilder = new SqlBuilder();
        var sqlTemplate = sqlBuilder.AddTemplate(sql);

        return (sqlBuilder, sqlTemplate);
    }

    internal static async Task<MeetingDTO> GetMeeting(
        IDbConnection connection, 
        Guid userId,
        Guid meetingId,
        double longitude,
        double latitude)
    {
        var (sqlBuilder, sqlTemplate) = CreateBaseQuery();
        
        sqlBuilder.Where("\"id\" = @MeetingId");

        var meeting = await connection.QuerySingleOrDefaultAsync<MeetingDTO>(
            sqlTemplate.RawSql,
            new { MeetingId = meetingId, Longitude = longitude, Latitude = latitude });

        if (meeting is null)
        {
            throw new ObjectNotFoundException($"{nameof(Meeting)} with Id: {meetingId} not found.");
        }

        if (!UserIsParticipant(meeting, userId))
        {
            meeting.ConfidentialInfo = null;
        }

        return meeting;
    }

    private static bool UserIsParticipant(MeetingDTO meeting, Guid userId)
        => meeting.Participants.Select(x => x.UserId).Contains(userId);

    internal static async Task<List<MeetingDTO>> GetMeetingsForExplore(
        IDbConnection connection,
        Guid userId,
        List<string> tags,
        double longitude,
        double latitude,
        int limit,
        int offset)
    {
        var excludedIds = await GetMeetingsIdsInWhichUserParticipates(connection, userId);
        
        var (sqlBuilder, sqlTemplate) = CreateBaseQuery();
        
        if (tags.Any())
        {
            sqlBuilder.Where("array[@Tags]::varchar[] &&\"tag_names\"");
        }
        
        sqlBuilder.Where("\"status\" = @Status");

        sqlBuilder.Where("NOT (\"id\" = ANY(@ExcludedIds))");
        
        sqlBuilder.OrderBy(
            "\"app\".\"calculate_distance_m\"(\"location_coordinates\", point(@Longitude, @Latitude)) asc, id");

        sqlTemplate = sqlBuilder.AddTemplate($"{sqlTemplate.RawSql} LIMIT @Limit OFFSET @Offset");

        var meetings = (await connection.QueryAsync<MeetingDTO>(
            sqlTemplate.RawSql,
            new
            {
                ExcludedIds = excludedIds,
                Tags = tags,
                Status = MeetingStatus.Upcoming.Name,
                Longitude = longitude,
                Latitude = latitude,
                Limit = limit,
                Offset = offset
            })).ToList();
        
        meetings.ForEach(x => x.ConfidentialInfo = null);

        return meetings;
    }

    internal static async Task<int> GetTotalNumberOfMeetingsForExplore(
        IDbConnection connection,
        Guid userId,
        List<string> tags,
        double longitude,
        double latitude)
    {
        var excludedIds = await GetMeetingsIdsInWhichUserParticipates(connection, userId);
        
        var (sqlBuilder, sqlTemplate) = CreateCountBaseQuery();
        
        if (tags.Any())
        {
            sqlBuilder.Where("array[@Tags]::varchar[] && \"tag_names\"");
        }
        
        sqlBuilder.Where("\"status\" = @Status");

        sqlBuilder.Where("NOT (\"id\" = ANY(@ExcludedIds))");
        
        return await connection.ExecuteScalarAsync<int>(
            sqlTemplate.RawSql,
            new
            {
                ExcludedIds = excludedIds,
                Tags = tags,
                Status = MeetingStatus.Upcoming.Name,
                Longitude = longitude,
                Latitude = latitude,
            });
    }

    private static async Task<List<Guid>> GetMeetingsIdsInWhichUserParticipates(
        IDbConnection connection, 
        Guid userId)
    {
        var sql = "select " +
                  "\"meeting_id\" " +

                  "FROM \"app\".\"v_participant\" " +
                  "/**where**/";

        var sqlBuilder = new SqlBuilder();
        var sqlTemplate = sqlBuilder.AddTemplate(sql);

        sqlBuilder.Where("\"user_id\" = @UserId");
        
        return (await connection.QueryAsync<Guid>(
            sqlTemplate.RawSql,
            new
            {
                UserId = userId
            })).ToList();
    }
    
    public static async Task<List<Guid>> GetMeetingsIdsWhichUserHosts(
        IDbConnection connection, 
        Guid userId)
    {
        var sql = "select " +
                  "\"meeting_id\" " +

                  "FROM \"app\".\"v_participant\" " +
                  "/**where**/";

        var sqlBuilder = new SqlBuilder();
        var sqlTemplate = sqlBuilder.AddTemplate(sql);

        sqlBuilder.Where("\"user_id\" = @UserId");
        sqlBuilder.Where("\"meeting_role\" = @MeetingRole");
        
        return (await connection.QueryAsync<Guid>(
            sqlTemplate.RawSql,
            new
            {
                UserId = userId,
                MeetingRole = MeetingRole.Owner.Name
            })).ToList();
    }
}