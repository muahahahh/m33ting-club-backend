using M33tingClub.Application.Utilities;
using M33tingClub.Domain.Tags;
using MediatR;

namespace M33tingClub.Application.Tags.MakeTagOfficial;

public class MakeTagOfficialCommandHandler : ICommandHandler<MakeTagOfficialCommand, Unit>
{
    private readonly ITagRepository _tagRepository;

    public MakeTagOfficialCommandHandler(ITagRepository tagRepository)
    {
        _tagRepository = tagRepository;
    }
    
    public async Task<Unit> Handle(MakeTagOfficialCommand command, CancellationToken cancellationToken)
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