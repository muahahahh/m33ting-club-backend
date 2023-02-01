using M33tingClub.Application.MeetingNotifications.GetUnseenMeetingNotifications;
using M33tingClub.Application.MeetingNotifications.MarkMeetingNotificationsAsSeen;
using M33tingClub.Application.Meetings.UploadImage.UserImage;
using M33tingClub.Application.Users;
using M33tingClub.Application.Users.DeleteUser;
using M33tingClub.Application.Users.FinishUserSignUp;
using M33tingClub.Application.Users.Followers.FollowUser;
using M33tingClub.Application.Users.Followers.GetFollowers;
using M33tingClub.Application.Users.Followers.GetFollowing;
using M33tingClub.Application.Users.Followers.UnfollowUser;
using M33tingClub.Application.Users.GetUser;
using M33tingClub.Application.Users.GetUserSelf;
using M33tingClub.Application.Utilities;
using M33tingClub.Domain.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace M33tingClub.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [SwaggerOperation(Description = "Gender values: Male, Female, Other.")]
    [HttpPost("sign-up")]
    [Authorize]
    public async Task<ActionResult> SignUp(SignUpCommand command)
        => Ok(await _mediator.Send(command));

    [HttpGet("{id:guid}")]
    [Authorize (Policy = Policies.UserExists)]
    public async Task<ActionResult<UserDetailsDTO>> Get([FromRoute] Guid id) 
        => Ok(await _mediator.Send(new GetUserQuery(id)));
    
    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<UserDTO>> Me() 
        => Ok(await _mediator.Send(new GetUserSelfQuery()));

    [HttpDelete]
    [Authorize(Policy = Policies.UserExists)]
    public async Task<ActionResult> Delete()
    {
        await _mediator.Send(new DeleteUserCommand());
        return NoContent();
    }
    
    [SwaggerOperation(Description = "Min. resolution 400x400, max. size 5Mb")]
    [HttpPost("avatar")]
    [Authorize]
    public async Task<ActionResult<ObjectCreatedResponse>> UploadUserImage(IFormFile file)
    {
        await using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        return await _mediator.Send(new UploadUserImageCommand(memoryStream));
    }

    [HttpPost("{id:guid}/follow")]
    [Authorize(Policy = Policies.UserExists)]
    public async Task<ActionResult> FollowUser([FromRoute] Guid id)
    {
        await _mediator.Send(new FollowUserCommand(id));
        return NoContent();
    }

    [HttpPost("{id:guid}/unfollow")]
    [Authorize(Policy = Policies.UserExists)]
    public async Task<ActionResult> UnfollowUser([FromRoute] Guid id)
    {
        await _mediator.Send(new UnfollowUserCommand(id));
        return NoContent();
    }

    [HttpGet("{id:guid}/followers")]
    [Authorize(Policy = Policies.UserExists)]
    public async Task<ActionResult<PagingInfo<FollowerDTO>>> GetFollowers([FromRoute] Guid id, [FromQuery] int limit = 10,
        [FromQuery] int offset = 0)
    {
        return Ok(await _mediator.Send(new GetFollowersQuery(id, limit, offset)));
    }

    [HttpGet("{id:guid}/following")]
    [Authorize(Policy = Policies.UserExists)]
    public async Task<ActionResult<PagingInfo<FollowingDTO>>> GetFollowing([FromRoute] Guid id, [FromQuery] int limit = 10,
        [FromQuery] int offset = 0)
    {
        return Ok(await _mediator.Send(new GetFollowingQuery(id, limit, offset)));
    }
    
    [HttpGet("meeting-notifications")]
    [Authorize(Policy = Policies.UserExists)]
    [SwaggerOperation(Description = "Type values: UserJoined, UserAskedToJoin, ApplicationAccepted.")]
    public async Task<ActionResult<List<MeetingNotificationDTO>>> GetUnseenMeetingNotifications([FromQuery] List<string>? types)
    {
        return Ok(await _mediator.Send(new GetUnseenMeetingNotificationsQuery(types)));
    }
    
    [HttpPatch("meeting-notifications/mark-as-seen")]
    [Authorize(Policy = Policies.UserExists)]
    public async Task<ActionResult> MarkAsSeenMeetingNotifications([FromBody] MarkMeetingNotificationsAsSeenCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }
}