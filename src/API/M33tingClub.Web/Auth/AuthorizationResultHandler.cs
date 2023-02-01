using M33tingClub.Domain.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace M33tingClub.Web.Auth;

public class AuthorizationResultHandler : IAuthorizationMiddlewareResultHandler
{
    private readonly IUserContext _userContext;

    private readonly AuthorizationMiddlewareResultHandler _defaultHandler = new();

    public AuthorizationResultHandler(IUserContext userContext)
    {
        _userContext = userContext;
    }

    public async Task HandleAsync(
        RequestDelegate next,
        HttpContext context,
        AuthorizationPolicy policy,
        PolicyAuthorizationResult authorizeResult)
    {
        if (authorizeResult.Succeeded)
        {
            await _userContext.InitializeContext();
        }
        
        await _defaultHandler.HandleAsync(next, context, policy, authorizeResult);
    }
}