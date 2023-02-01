using FirebaseAdmin.Auth;
using M33tingClub.Domain.Auth;
using M33tingClub.Domain.Utilities.Exceptions;
using Microsoft.AspNetCore.Authentication;

namespace M33tingClub.Web.Auth;

public class UserContext : IUserContext
{
    private FirebaseToken? _decodedToken;

    public Guid UserId => UserIdOptional ?? throw new UserNotExistsException(FirebaseId);

    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public Guid? UserIdOptional
    {
        get
        {
            if (_decodedToken is not null && _decodedToken.Claims.TryGetValue("dbUserId", out var dbUserId))
            {
                return new Guid((string)dbUserId);
            }

            return null;
        }
    }

    public string FirebaseId
    {
        get
        {
            if (_decodedToken is not null && _decodedToken.Claims.TryGetValue("user_id", out var firebaseId))
            {
                return (firebaseId as string)!;
            }

            return string.Empty;
        }
    }

    public async Task InitializeContext()
    {
        if (_httpContextAccessor.HttpContext is null)
        {
            throw new Exception("Access Token could not be found!");
        }
        
        var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
        _decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(accessToken);
    }
}