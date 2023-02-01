using System.Data;
using M33tingClub.Application.Utilities;
using M33tingClub.Domain.Auth;

namespace M33tingClub.Application.Meetings.GetMeeting;

public class GetMeetingQueryHandler : IQueryHandler<GetMeetingQuery, MeetingDTO?>
{
    private readonly IDbConnection _connection;
    private readonly IUserContext _userContext;
    
    public GetMeetingQueryHandler(ISqlConnectionFactory sqlConnectionFactory, IUserContext userContext)
    {
        _userContext = userContext;
        _connection = sqlConnectionFactory.GetOpenConnection();
    }

    public async Task<MeetingDTO?> Handle(GetMeetingQuery query, CancellationToken cancellationToken)
        => await MeetingsSupplier.GetMeeting(
            _connection,
            _userContext.UserId,
            query.MeetingId, 
            query.Longitude, 
            query.Latitude);
}

