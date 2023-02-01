using M33tingClub.Application.Utilities;
using M33tingClub.Domain.Tags;
using MediatR;

namespace M33tingClub.Application.Tags.MakeTagCommunity;

public class MakeTagCommunityCommandHandler : ICommandHandler<MakeTagCommunityCommand, Unit>
{
    private readonly ITagRepository _tagRepository;

    public MakeTagCommunityCommandHandler(ITagRepository tagRepository)
    {
        _tagRepository = tagRepository;
    }
    
    public async Task<Unit> Handle(MakeTagCommunityCommand command, CancellationToken cancellationToken)
    {
        var tagName = TagName.Create(command.Name);
        var tag = await _tagRepository.Get(tagName);
        if (tag is null)
        {
            throw new Exception(); //Create specific exception
        }

        tag.MakeCommunity();
        
        return await Unit.Task;
    }
}