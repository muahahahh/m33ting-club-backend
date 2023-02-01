using M33tingClub.Application.Utilities;

namespace M33tingClub.Application.Tags.SearchTags;

public record SearchTagsQuery(
    string? Phrase, 
    int Limit, 
    int Offset) : IQuery<PagingInfo<TagDTO>>;