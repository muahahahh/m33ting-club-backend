using Dapper;
using M33tingClub.Application.MeetingNotifications;
using M33tingClub.Application.Meetings;
using M33tingClub.Application.Tags;
using M33tingClub.Application.Users;
using M33tingClub.Application.Users.Followers;
using M33tingClub.Application.Utilities;
using M33tingClub.Infrastructure.Configuration.Dapper;
using M33tingClub.Infrastructure.DataAccess;
using M33tingClub.Infrastructure.DataAccess.Dapper;
using M33tingClub.Infrastructure.MeetingNotifications;
using M33tingClub.Infrastructure.Meetings;
using M33tingClub.Infrastructure.Tags;
using M33tingClub.Infrastructure.Users;
using M33tingClub.Infrastructure.Users.Followers;
using Microsoft.EntityFrameworkCore;

namespace M33tingClub.Web.Registrations;

public static class DataAccessRegistration
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services, M33tingClubConfiguration configuration)
    {
        services.AddDbContext<M33tingClubDbContext>(
            x => x.UseNpgsql(configuration.DatabaseConnectionString));

        services.AddScoped<ISqlConnectionFactory>(
            _ => new NpgsqlConnectionFactory(configuration.DatabaseConnectionString));

        services.AddScoped<IMeetingRepository, MeetingRepository>();
        services.AddScoped<ITagRepository, TagRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IFollowershipRepository, FollowershipRepository>();
        services.AddScoped<IMeetingNotificationRepository, MeetingNotificationRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        SqlMapper.AddTypeHandler(new ListHandler<string>());
        SqlMapper.AddTypeHandler(new JsonHandler<ParticipantDTO>());
        
        return services;
    }
}