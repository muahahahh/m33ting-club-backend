using System.Data;
using System.Diagnostics;
using Dapper;
using M33tingClub.Application.Utilities;
using M33tingClub.Domain.Auth;

namespace M33tingClub.Application.Meetings.GetOwnMeetings;

public class GetOwnMeetingsQueryHandler : IQueryHandler<GetOwnMeetingsQuery, List<MeetingDTO>>
{
    private readonly IDbConnection _connection;
    private readonly IUserContext _userContext;

    public GetOwnMeetingsQueryHandler(ISqlConnectionFactory sqlConnectionFactory, IUserContext userContext)
    {
        _connection = sqlConnectionFactory.GetOpenConnection();
        _userContext = userContext;
    }
        
    public async Task<List<MeetingDTO>> Handle(GetOwnMeetingsQuery query, CancellationToken cancellationToken)
    {
        var sql = "select " +
                  $"\"meeting\".\"id\" as {nameof(MeetingDTO.Id)}, " +
                  $"\"meeting\".\"name\" as {nameof(MeetingDTO.Name)}, " +
                  $"\"meeting\".\"description\" as {nameof(MeetingDTO.Description)}, " +
                  $"\"meeting\".\"participants_limit\" as {nameof(MeetingDTO.ParticipantsLimit)}, " +
                  $"\"meeting\".\"start_date\" as {nameof(MeetingDTO.StartDate)}, " +
                  $"\"meeting\".\"end_date\" as {nameof(MeetingDTO.EndDate)}, " +
                  $"\"meeting\".\"image_id\" as {nameof(MeetingDTO.ImageId)}, " +
                  $"\"meeting\".\"location_name\" as {nameof(MeetingDTO.LocationName)}, " +
                  $"\"meeting\".\"location_description\" as {nameof(MeetingDTO.LocationDescription)}, " +
                  $"\"meeting\".\"location_coordinates\"[0] as {nameof(MeetingDTO.Longitude)}, " +
                  $"\"meeting\".\"location_coordinates\"[1] as {nameof(MeetingDTO.Latitude)}, " +
                  $"\"meeting\".\"status\" as {nameof(MeetingDTO.Status)}, " +
                  $"\"meeting_tag\".\"tag_names\" as {nameof(MeetingDTO.Tags)}, " +
                  $"\"participant\".\"participant\" as {nameof(MeetingDTO.Participants)}, " +
                  $"\"meeting\".\"is_public\" as {nameof(MeetingDTO.IsPublic)} " +
                  "from \"app\".\"meeting\" " +
                  "left join ( " +
                  "select  " +
                  "\"meeting_id\", " +
                  "array_agg(\"tag_name\") as \"tag_names\" " +
                  "from \"app\".\"v_meeting_tag\" " +
                  "group by \"meeting_id\" ) as \"meeting_tag\" on \"app\".\"meeting\".\"id\" = \"meeting_tag\".\"meeting_id\" " +
                  "inner join ( " +
                  "select " +
                  "\"meeting_id\", " +
                  "json_agg(json_build_object('user_id', \"user_id\" , 'name', \"name\", 'image_id', \"image_id\", 'meeting_role' , \"meeting_role\")) as \"participant\" " +
                  "from \"app\".\"v_participant\" " +
                  "/**where**/ " +
                  "group by \"meeting_id\" ) as \"participant\" on \"app\".\"meeting\".id = \"participant\".\"meeting_id\" " +
                  "where \"meeting\".\"status\" = ANY(@Statuses) " +
                  "order by abs(extract(epoch from \"meeting\".\"start_date\" - now())) asc, meeting.id " +
                  "limit @Limit offset @Offset";

        var userId = _userContext.UserId;
        var statuses = query.Statuses;
        var role = query.Role;
        var limit = query.Limit;
        var offset = query.Offset;
        
        var sqlBuilder = new SqlBuilder();
        var sqlTemplate = sqlBuilder.AddTemplate(sql);
        
        sqlBuilder.Where("\"user_id\" = @UserId ");

        if (!string.IsNullOrEmpty(role))
        {
            sqlBuilder.Where("\"meeting_role\" = @Role ");
        }

        return (await _connection.QueryAsync<MeetingDTO>(sqlTemplate.RawSql, 
            new 
            {   UserId = userId, 
                Statuses = statuses,
                Role = role,
                Limit = limit,
                Offset = offset
            })).ToList();
    }
}