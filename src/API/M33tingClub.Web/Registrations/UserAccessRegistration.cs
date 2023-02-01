using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using M33tingClub.Domain.Auth;
using M33tingClub.Web.Auth;
using M33tingClub.Web.Auth.Handlers;
using M33tingClub.Web.Auth.Requirements;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace M33tingClub.Web.Registrations;

public static class UserAccessRegistration
{
    public static IServiceCollection AddUserAccess(this IServiceCollection services, M33tingClubConfiguration configuration)
    {
        FirebaseApp.Create(new AppOptions
        {
            Credential = GoogleCredential.FromFile(configuration.FirebaseCredentialsPath)
        });

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = $"https://securetoken.google.com/{configuration.FirebaseProjectId}";
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = $"https://securetoken.google.com/{configuration.FirebaseProjectId}",
                    ValidateAudience = true,
                    ValidAudience = configuration.FirebaseProjectId,
                    ValidateLifetime = true
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy(Policies.UserExists, 
                policy => policy.Requirements.Add(new UserExistsRequirement()));
        });
        
        services.AddScoped<IUserContext, UserContext>();

        services.AddSingleton<IAuthorizationHandler, UserExistsHandler>();
        
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddScoped<IAuthorizationMiddlewareResultHandler, AuthorizationResultHandler>();
        
        return services;
    }
}