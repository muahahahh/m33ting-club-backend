using M33tingClub.Infrastructure.Scheduling;
using Quartz;

namespace M33tingClub.Web.Registrations;

public static class JobSchedulingRegistration
{
	public static IServiceCollection AddJobScheduling(this IServiceCollection services)
	{
		services.AddQuartz(quartz =>
		{
			quartz.UseMicrosoftDependencyInjectionJobFactory();
            
			quartz.UseSimpleTypeLoader();
			quartz.UseInMemoryStore();
			quartz.UseDefaultThreadPool(tp =>
			{
				tp.MaxConcurrency = 10;
			});
            
			quartz.ScheduleJob<UpdateMeetingsStatusesJob>(trigger =>
			{
				trigger.WithIdentity("trigger", "group")
				       .StartNow()
				       .WithCronSchedule("0 0/1 * * * ?");
			});
		});
        
		services.AddQuartzHostedService();

		return services;
	}
}