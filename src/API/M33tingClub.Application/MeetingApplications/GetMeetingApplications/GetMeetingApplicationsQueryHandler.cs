using System.Data;
using Dapper;
using M33tingClub.Application.MeetingApplications;
using M33tingClub.Application.Utilities;
using M33tingClub.Domain.Auth;

namespace M33tingClub.Application.MeetingApplications.GetMeetingApplications;

public class GetMeetingApplicationsQueryHandler : IQueryHandler<GetMeetingApplicationsQuery, List<ApplicationDTO>>
{
    private readonly IDbConnection _connection;
    private readonly IUserContext _userContext;

    public GetMeetingApplicationsQueryHandler(ISqlConnectionFactory sqlConnectionFactory, IUserContext userContext)
    {
        _connection = sqlConnectionFactory.GetOpenConnection();
        _userContext = userContext;
    }
    
    public async Task<List<ApplicationDTO>> Handle(GetMeetingApplicationsQuery query, CancellationToken cancellationToken)
    {
        var sql = "select " +
        $"\"meeting_id\" as {nameof(ApplicationDTO.MeetingId)}, " +
        $"\"user_id\" as {nameof(ApplicationDTO.UserId)}, " +
        $"\"status\" as {nameof(ApplicationDTO.Status)}, " +
        $"\"name\" as {nameof(ApplicationDTO.Name)}, " +
        $"\"image_id\" as {nameof(ApplicationDTO.ImageId)} " +
        "from \"app\".\"v_application\" " +
        "where \"meeting_id\" = @MeetingId";

        var meetingId = query.MeetingId;
        
        return (await _connection.QueryAsync<ApplicationDTO>(sql,
            new
            {
                MeetingId = meetingId
            })).ToList();
    }
}