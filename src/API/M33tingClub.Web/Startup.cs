using Amazon.S3;
using M33tingClub.Infrastructure.Processing;
using M33tingClub.Web.Registrations;

namespace M33tingClub.Web;

public class Startup
{
    private readonly IConfiguration _configuration;
    private readonly M33tingClubConfiguration _m33tingClubConfiguration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
        _m33tingClubConfiguration = new M33tingClubConfiguration(_configuration);
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddLogging();

        services
            .AddSingleton(_m33tingClubConfiguration)
            .AddExceptionHandling()
            .AddSwagger()
            .AddProcessing()
            .AddDataAccess(_m33tingClubConfiguration)
            .AddUserAccess(_m33tingClubConfiguration)
            .AddAWSService<IAmazonS3>()
            .AddAWSS3Access(_m33tingClubConfiguration)
            .AddJobScheduling()
            .Configure<RouteOptions>(
                options => { options.LowercaseUrls = true; });
    }

    public void Configure(IApplicationBuilder app)
    {
        app
            .UseSwagger()
            .UseSwaggerUI(
                options => { options.DefaultModelsExpandDepth(-1); })
            .UseHttpsRedirection()
            .UseExceptionHandling()
            .UseRouting()
            .UseAuthentication()
            .UseAuthorization()
            .UseEndpoints(x => x.MapControllers());
    }
}