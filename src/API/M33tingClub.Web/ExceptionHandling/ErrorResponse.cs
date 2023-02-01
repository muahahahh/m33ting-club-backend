using System.Net;

namespace M33tingClub.Web.ExceptionHandling;

public class ErrorResponse
{
	//TODO: there should be no setters
	public string[] Errors { get; set; }
	
	public HttpStatusCode StatusCode { get; set; }

	public ErrorResponse(string error, HttpStatusCode statusCode)
	{
		Errors = new[] { error };
		StatusCode = statusCode;
	}
	
	public ErrorResponse(IEnumerable<string> errors, HttpStatusCode statusCode)
	{
		Errors = errors.ToArray();
		StatusCode = statusCode;
	}

	//it is only used for deserialization
	//I will consider switching to NewtonJsonSoft to be able to make it private
	public ErrorResponse()
	{
		
	}
}