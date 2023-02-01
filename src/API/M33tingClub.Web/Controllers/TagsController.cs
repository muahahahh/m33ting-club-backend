using M33tingClub.Application.Tags;
using M33tingClub.Application.Tags.AddCommunityTag;
using M33tingClub.Application.Tags.AddOfficialTag;
using M33tingClub.Application.Tags.GetTag;
using M33tingClub.Application.Tags.GetTags;
using M33tingClub.Application.Tags.MakeTagCommunity;
using M33tingClub.Application.Tags.MakeTagOfficial;
using M33tingClub.Application.Tags.SearchTags;
using M33tingClub.Application.Utilities;
using M33tingClub.Domain.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace M33tingClub.Web.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize (Policy = Policies.UserExists)]
public class TagsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TagsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{name}")]
    [ProducesResponseType(typeof(TagDTO), StatusCodes.Status200OK)]
    public async Task<ActionResult<TagDTO>> GetTag([FromRoute] string name)
        => Ok(await _mediator.Send(new GetTagQuery(name)));

    [HttpGet]
    [ProducesResponseType(typeof(List<TagDTO>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<TagDTO>>> GetTags([FromQuery] bool? isOfficial)
        => Ok(await _mediator.Send(new GetTagsQuery(isOfficial)));

    [HttpGet("search")]
    [ProducesResponseType(typeof(PagingInfo<TagDTO>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagingInfo<TagDTO>>> Search([FromQuery] string? phrase,
        [FromQuery] int limit = 10, [FromQuery] int offset = 0)
        => Ok(await _mediator.Send(new SearchTagsQuery(phrase, limit, offset)));
    

    //TODO: Only for admin
    [HttpPost("official")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<NamedObjectCreatedResponse>> AddOfficial([FromBody] AddOfficialTagCommand command)
    {
        var response = await _mediator.Send(command);
        return Created($"tags/{response.Name}", response);
    }

    //TODO: Only for admin
    [HttpPost("community")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<NamedObjectCreatedResponse>> AddCommunity([FromBody] AddCommunityTagCommand command)
    {
        var response = await _mediator.Send(command);
        return Created($"tags/{response.Name}", response);
    }

    //TODO: Only for admin
    [HttpPut("official")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> MakeOfficial([FromBody] string name)
    {
        await _mediator.Send(new MakeTagOfficialCommand(name));
        return NoContent();
    }
    
    //TODO: Only for admin
    [HttpPut("community")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> MakeCommunity([FromBody] string name)
    {
        await _mediator.Send(new MakeTagCommunityCommand(name));
        return NoContent();
    }
}