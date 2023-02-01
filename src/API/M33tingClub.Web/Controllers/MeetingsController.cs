using M33tingClub.Application.Meetings;
using M33tingClub.Application.Meetings.AddMeeting;
using M33tingClub.Application.Meetings.CancelMeeting;
using M33tingClub.Application.Meetings.EditMeeting;
using M33tingClub.Application.Meetings.GetFeed;
using M33tingClub.Application.Meetings.GetMeeting;
using M33tingClub.Application.Meetings.GetMeetings;
using M33tingClub.Application.Meetings.GetOwnMeetings;
using M33tingClub.Application.Meetings.JoinMeeting;
using M33tingClub.Application.Meetings.LeaveMeeting;
using M33tingClub.Application.Meetings.UploadImage.MeetingBackgroundImage;
using M33tingClub.Application.Utilities;
using M33tingClub.Domain.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace M33tingClub.Web.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Policy = Policies.UserExists)]
public class MeetingsController : ControllerBase
{
    private readonly IMediator _mediator;

    public MeetingsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(PagingInfo<MeetingDTO>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagingInfo<MeetingDTO>>> GetMeetingsForExplore([FromQuery] List<string> tags, 
        [FromQuery] double longitude, [FromQuery] double latitude,
        [FromQuery] int limit = 10, [FromQuery] int offset = 0)
        => Ok(await _mediator.Send(new GetMeetingsForExploreQuery(
                    tags, longitude, latitude, limit, offset)));

    [SwaggerOperation(Description = "Possible status values: Cancelled, Finished, Ongoing, Upcoming. " +
                                    "Possible role values: Owner, Leader, Member.")]
    [HttpGet("own")]
    [ProducesResponseType(typeof(PagingInfo<MeetingDTO>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagingInfo<MeetingDTO>>> GetOwn([FromQuery] string? role, [FromQuery] List<string> statuses,
        [FromQuery] int limit = 10, [FromQuery] int offset = 0)
        => Ok(await _mediator.Send(new GetOwnMeetingsQuery(statuses, role, limit, offset)));

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(MeetingDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MeetingDTO>> Get(
        [FromRoute] Guid id,
        [FromQuery] double longitude,
        [FromQuery] double latitude)
        => Ok(await _mediator.Send(new GetMeetingQuery(id, longitude, latitude)));

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<ObjectCreatedResponse>> Add([FromBody] AddMeetingCommand command)
    {
        var response = await _mediator.Send(command);
        return Created($"meetings/{response.Id}", response);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Edit([FromBody] EditMeetingCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPatch("{id:guid}/cancel")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Cancel([FromRoute] Guid id)
    {
        var command = new CancelMeetingCommand(id);
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPost("{id:guid}/join")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Join([FromRoute] Guid id)
    {
        var command = new JoinMeetingCommand(id);
        await _mediator.Send(command);
        return NoContent();
    }
    
    [HttpPost("{id:guid}/leave")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Leave([FromRoute] Guid id)
    {
        var command = new LeaveMeetingCommand(id);
        await _mediator.Send(command);
        return NoContent();
    }

    [SwaggerOperation(Description = "Min. resolution 780x780, max. size 5Mb")]
    [HttpPost("backgrounds")]
    public async Task<ActionResult<ObjectCreatedResponse>> UploadBackgroundImage(IFormFile file)
    {
        await using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        return await _mediator.Send(new UploadMeetingBackgroundImageCommand(memoryStream));
    }

    [HttpGet("feed")]
    [ProducesResponseType(typeof(PagingInfo<FeedDTO>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagingInfo<FeedDTO>>> GetFeed([FromQuery] int limit = 10, [FromQuery] int offset = 0)
    {
        return Ok(await _mediator.Send(new GetFeedQuery(limit, offset)));
    }
}