using M33tingClub.Application.Utilities;
using M33tingClub.Domain.Tags;

namespace M33tingClub.Application.Tags.AddOfficialTag;

public class AddOfficialTagCommandHandler : ICommandHandler<AddOfficialTagCommand, NamedObjectCreatedResponse>
{
    private readonly ITagRepository _tagRepository;

    public AddOfficialTagCommandHandler(ITagRepository tagRepository)
    {
        _tagRepository = tagRepository;
    }

    public async Task<NamedObjectCreatedResponse> Handle(AddOfficialTagCommand command, CancellationToken cancellationToken)
    {
        var tagName = TagName.Create(command.Name);
        var tag = Tag.CreateOfficial(tagName);

        await _tagRepository.Add(tag);

        return new NamedObjectCreatedResponse(tag.Name.Value);
    }
}