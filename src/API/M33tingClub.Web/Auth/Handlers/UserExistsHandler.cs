using FirebaseAdmin.Auth;
using M33tingClub.Domain.Utilities.Exceptions;
using M33tingClub.Web.Auth.Requirements;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace M33tingClub.Web.Auth.Handlers;

public class UserExistsHandler : AuthorizationHandler<UserExistsRequirement>
{
    private readonly IHttpContextAccessor _contextAccessor;
    
    public UserExistsHandler(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }
    
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, UserExistsRequirement requirement)
    {
        if (_contextAccessor.HttpContext is null)
        {
            context.Fail();
            return;
        }

        var accessToken = await _contextAccessor.HttpContext.GetTokenAsync("access_token");

        if (string.IsNullOrEmpty(accessToken))
        {
            context.Fail();
            return;
        }
        
        var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(accessToken);
        
        if (decodedToken.Claims.ContainsKey("dbUserId"))
        {
            context.Succeed(requirement);
            return;
        }
                
        if (decodedToken.Claims.TryGetValue("user_id", out var firebaseId))
        {
            throw new UserNotExistsException((string)firebaseId);
        }

        context.Fail();
    }
}