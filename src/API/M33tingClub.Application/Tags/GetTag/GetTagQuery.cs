using M33tingClub.Application.Utilities;

namespace M33tingClub.Application.Tags.GetTag;

public record GetTagQuery(string Name) : IQuery<TagDTO>;