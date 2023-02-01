using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using M33tingClub.Web.ExceptionHandling;

namespace M33tingClub.IntegrationTests.Utilities.Clients;

internal class M33tingClubResponse<T>
{
	public readonly HttpStatusCode StatusCode;

	public T Content => _content ?? throw new Exception($"There is no content of type {typeof(T)} in response.");

	private readonly T? _content;

	public ErrorResponse ErrorContent => _errorContent ?? throw new Exception("There is no ERROR content in response.");
	
	private readonly ErrorResponse? _errorContent;

	public M33tingClubResponse(HttpStatusCode statusCode, HttpContent httpContent)
	{
		StatusCode = statusCode;
		
		//TODO: Log url, status, body
		var s = httpContent.ReadAsStringAsync().GetAwaiter().GetResult();
		if (statusCode.IsSuccessful())
		{
			_content = JsonSerializer.Deserialize<T>(s, 
				new JsonSerializerOptions { PropertyNameCaseInsensitive = true});
		}
		else
		{
			_errorContent = JsonSerializer.Deserialize<ErrorResponse>(s, 
				new JsonSerializerOptions { PropertyNameCaseInsensitive = true});
		}
	}
}

internal class M33tingClubResponse
{
	public readonly HttpStatusCode StatusCode;

	public ErrorResponse ErrorContent => _errorContent ?? throw new Exception("There is no ERROR content in response.");
	
	private readonly ErrorResponse? _errorContent;

	public M33tingClubResponse(HttpStatusCode statusCode, HttpContent httpContent)
	{
		StatusCode = statusCode;

		//TODO: Log url, status, body
		var s = httpContent.ReadAsStringAsync().GetAwaiter().GetResult();
		if (!statusCode.IsSuccessful())
		{
			_errorContent = JsonSerializer.Deserialize<ErrorResponse>(s, 
				new JsonSerializerOptions { PropertyNameCaseInsensitive = true});
		}
	}
}