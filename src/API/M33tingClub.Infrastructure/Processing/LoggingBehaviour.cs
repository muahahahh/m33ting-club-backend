using System.Text;
using M33tingClub.Domain.Auth;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace M33tingClub.Infrastructure.Processing;

public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;
    private readonly IUserContext _userContext;

    public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger, IUserContext userContext)
    {
        _logger = logger;
        _userContext = userContext;
    }
    
    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var userId = _userContext.UserIdOptional;
        var userInfo = userId is not null
            ? $"[UserId: {userId}] "
            : string.Empty;
        
        try
        {
            _logger.LogInformation($"{userInfo}Executing {request.GetType().Name} started.");
            var result = await next();
            _logger.LogInformation($"{userInfo}Executing {request.GetType().Name} succeeded.");
            return result;
        }
        catch (Exception e)
        {
            
            _logger.LogError($"{userInfo}Executing {request.GetType().Name} failed. {Environment.NewLine}" +
                             $"Request: {(request.GetType().IsSerializable ? JsonConvert.SerializeObject(request) : "Not serializable")} {Environment.NewLine}" +
                $"Message: {SerializeException(e)}");
            
            throw;
        }
    }

    private string SerializeException(Exception e)
    {
        var sb = new StringBuilder();
        sb.Append($"{e.Message}");
        
        var currentException = e.InnerException;
        while (currentException is not null)
        {
            sb.Append($"{Environment.NewLine} -> {currentException.Message}");
            currentException = currentException.InnerException;
        }

        return sb.ToString();
    }
}