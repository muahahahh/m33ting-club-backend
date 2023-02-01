using M33tingClub.Web.ExceptionHandling;

namespace M33tingClub.Web.Registrations;

internal static class ExceptionHandlingRegistration
{
	internal static IServiceCollection AddExceptionHandling(this IServiceCollection services)
	{
		services.AddScoped<ExceptionHandlingMiddleware>();

		return services;
	}

	internal static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app)
	{
		app.UseMiddleware<ExceptionHandlingMiddleware>();

		return app;
	}
}