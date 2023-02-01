using System.Data;
using Dapper;
using M33tingClub.Application.Utilities;
using M33tingClub.Domain.Auth;

namespace M33tingClub.Application.MeetingNotifications.GetUnseenMeetingNotifications;

public class GetUnseenMeetingNotificationsQueryHandler 
    : IQueryHandler<GetUnseenMeetingNotificationsQuery, List<MeetingNotificationDTO>>
{
    private readonly IDbConnection _connection;
    private readonly IUserContext _userContext;

    public GetUnseenMeetingNotificationsQueryHandler(
        ISqlConnectionFactory connectionFactory, 
        IUserContext userContext)
    {
        _connection = connectionFactory.GetOpenConnection();
        _userContext = userContext;
    }
    
    public async Task<List<MeetingNotificationDTO>> Handle(GetUnseenMeetingNotificationsQuery query, CancellationToken cancellationToken)
    {
        var currentUserId = _userContext.UserId;

        var sql = "select " +
                  $"\"id\" as {nameof(MeetingNotificationDTO.Id)}, " +
                  $"\"type\" as {nameof(MeetingNotificationDTO.Type)}, " +
                  $"\"meeting_id\" as {nameof(MeetingNotificationDTO.MeetingId)}, " +
                  $"\"occured_on\" as {nameof(MeetingNotificationDTO.OccuredOn)}, " +
                  $"\"performer_id\" as {nameof(MeetingNotificationDTO.PerformerId)}, " +
                  $"\"performer_name\" as {nameof(MeetingNotificationDTO.PerformerName)} " +
                  "FROM \"app\".\"v_meeting_notification\" " +
                  "/**where**/";

        var sqlBuilder = new SqlBuilder();
        var sqlTemplate = sqlBuilder.AddTemplate(sql);
        
        sqlBuilder.Where("\"was_seen\" = false");
        sqlBuilder.Where("\"receiver_id\" = @ReceiverId");
        
        if (query.Types is not null && query.Types.Any())
        {
            sqlBuilder.Where("\"type\" = ANY(@Types)");
        }

        return (await _connection.QueryAsync<MeetingNotificationDTO>(
            sqlTemplate.RawSql,
            new
            {
                ReceiverId = currentUserId,
                Types = query.Types
            })).ToList();
    }
}