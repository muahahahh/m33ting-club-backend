using M33tingClub.Application.Utilities;

namespace M33tingClub.Application.Tags.GetTags;

public record GetTagsQuery(bool? IsOfficial) : IQuery<List<TagDTO>>;
