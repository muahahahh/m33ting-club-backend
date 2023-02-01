using M33tingClub.Application.Utilities;

namespace M33tingClub.Application.Meetings.GetFeed;

public record GetFeedQuery(
    int Limit,
    int Offset) : IQuery<PagingInfo<FeedDTO>>;