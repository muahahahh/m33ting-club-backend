using M33tingClub.Application.MeetingApplications;
using M33tingClub.Application.MeetingApplications.AcceptMeetingApplication;
using M33tingClub.Application.MeetingApplications.GetMeetingApplications;
using M33tingClub.Application.MeetingApplications.RejectMeetingApplication;
using M33tingClub.Application.Utilities;
using M33tingClub.Domain.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace M33tingClub.Web.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Policy = Policies.UserExists)]
public class ApplicationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ApplicationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{meetingId:guid}")]
    [ProducesResponseType(typeof(List<ApplicationDTO>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ApplicationDTO>>> GetMeetingApplications([FromRoute] Guid meetingId)
        => Ok(await _mediator.Send(new GetMeetingApplicationsQuery(meetingId)));


    [HttpPatch("{meetingId:guid}/user/{userId:guid}/accept")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult> AcceptMeetingApplication([FromRoute] Guid meetingId,
        [FromRoute] Guid userId)
    {
        var command = new AcceptMeetingApplicationCommand(meetingId, userId);
        return Ok(await _mediator.Send(command));
    }
    
    [HttpPatch("{meetingId:guid}/user/{userId:guid}/reject")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult> RejectMeetingApplication([FromRoute] Guid meetingId,
        [FromRoute] Guid userId)
    {
        var command = new RejectMeetingApplicationCommand(meetingId, userId);
        return Ok(await _mediator.Send(command));
    }
}