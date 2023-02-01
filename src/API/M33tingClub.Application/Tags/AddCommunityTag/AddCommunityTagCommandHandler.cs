using M33tingClub.Application.Utilities;
using M33tingClub.Domain.Tags;

namespace M33tingClub.Application.Tags.AddCommunityTag;

public class AddCommunityTagCommandHandler : ICommandHandler<AddCommunityTagCommand, NamedObjectCreatedResponse>
{
    private readonly ITagRepository _tagRepository;

    public AddCommunityTagCommandHandler(ITagRepository tagRepository)
    {
        _tagRepository = tagRepository;
    }

    public async Task<NamedObjectCreatedResponse> Handle(AddCommunityTagCommand command, CancellationToken cancellationToken)
    {
        var tagName = TagName.Create(command.Name);
        var tag = Tag.CreateCommunity(tagName);

        await _tagRepository.Add(tag);

        return new NamedObjectCreatedResponse(tag.Name.Value);
    }
}