using FluentValidation.AspNetCore;
using M33tingClub.Application.Meetings.GetMeetings;
using M33tingClub.Application.Utilities;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace M33tingClub.Infrastructure.Processing;

public static class ProcessingRegistration
{
    public static IServiceCollection AddProcessing(this IServiceCollection services)
    {
        services.AddMediatR(typeof(ICommand<>).Assembly);
        
        services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly(typeof(GetMeetingsForExploreQuery).Assembly));
        
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));
        
        return services;
    }
}