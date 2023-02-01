using System.Data;
using M33tingClub.Application.Utilities;
using M33tingClub.Domain.Auth;

namespace M33tingClub.Application.Meetings.GetMeetings;

public class GetMeetingsForExploreQueryHandler : IQueryHandler<GetMeetingsForExploreQuery, PagingInfo<MeetingDTO>>
{
    private readonly IDbConnection _connection;
    
    private readonly IUserContext _userContext;

    public GetMeetingsForExploreQueryHandler(
        ISqlConnectionFactory sqlConnectionFactory, 
        IUserContext userContext)
    {
        _userContext = userContext;
        _connection = sqlConnectionFactory.GetOpenConnection();
    }

    public async Task<PagingInfo<MeetingDTO>> Handle(GetMeetingsForExploreQuery forExploreQuery, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        
        var meetings = await MeetingsSupplier.GetMeetingsForExplore(
            _connection,
            userId,
            forExploreQuery.Tags,
            forExploreQuery.Longitude,
            forExploreQuery.Latitude,
            forExploreQuery.Limit,
            forExploreQuery.Offset);

        var totalNumberOfMeetings = await MeetingsSupplier.GetTotalNumberOfMeetingsForExplore(
            _connection,
            userId,
            forExploreQuery.Tags,
            forExploreQuery.Longitude,
            forExploreQuery.Latitude);

        return new PagingInfo<MeetingDTO>(meetings, totalNumberOfMeetings);
    }
}