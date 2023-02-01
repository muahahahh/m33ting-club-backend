
namespace M33tingClub.Web.ExceptionHandling;

internal class ExceptionHandlingMiddleware : IMiddleware
{
	public async Task InvokeAsync(HttpContext context, RequestDelegate next)
	{
		try
		{
			await next(context);
		}
		catch (Exception e)
		{
			await HandleExceptionAsync(context, e);
		}
	}

	private async Task HandleExceptionAsync(HttpContext context, Exception e)
	{
		var response = e.ToErrorResponse();

		context.Response.StatusCode = (int)response.StatusCode;
		await context.Response.WriteAsJsonAsync(response);
	}
}