using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace M33tingClub.IntegrationTests.Utilities.Clients;

internal abstract class M33tingClubClientBase
{
	private readonly HttpClient _httpClient;

	protected M33tingClubClientBase(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}
	
	protected async Task<M33tingClubResponse<T>> Get<T>(string url, string? authToken = null)
	{
		using (var request = new HttpRequestMessage(HttpMethod.Get, url))
		{
			if (authToken is not null)
			{
				request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
			}
			
			var response = await _httpClient.SendAsync(request);

			return new M33tingClubResponse<T>(response.StatusCode, response.Content);
		}
	}

	protected async Task<M33tingClubResponse<T>> Post<TRequest, T>(string url, TRequest request, string? authToken = null)
	{
		var data = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

		using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, url))
		{
			httpRequest.Content = data;
			if (authToken is not null)
			{
				httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
			}

			var response = await _httpClient.SendAsync(httpRequest);

			return new M33tingClubResponse<T>(response.StatusCode, response.Content);
		}
	}

	protected async Task<M33tingClubResponse> Post<TRequest>(string url, TRequest request, string? authToken = null)
	{
		var data = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
		
		using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, url))
		{
			httpRequest.Content = data;
			if (authToken is not null)
			{
				httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
			}

			var response = await _httpClient.SendAsync(httpRequest);

			return new M33tingClubResponse(response.StatusCode, response.Content);
		}
	}
	
	protected async Task<M33tingClubResponse> Post(string url, string? authToken = null)
	{
		using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, url))
		{
			if (authToken is not null)
			{
				httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
			}

			var response = await _httpClient.SendAsync(httpRequest);

			return new M33tingClubResponse(response.StatusCode, response.Content);
		}
	}
	
	protected async Task<M33tingClubResponse> Patch<TRequest>(string url, TRequest request, string? authToken = null)
	{
		var data = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
		
		using (var httpRequest = new HttpRequestMessage(HttpMethod.Patch, url))
		{
			httpRequest.Content = data;
			if (authToken is not null)
			{
				httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
			}

			var response = await _httpClient.SendAsync(httpRequest);

			return new M33tingClubResponse(response.StatusCode, response.Content);
		}
	}
	
	protected async Task<M33tingClubResponse<T>> PostFile<T>(string url, MemoryStream stream, string? authToken = null)
	{
		var data = new StreamContent(stream);
		data.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");

		var multipartFormContent = new MultipartFormDataContent();
		multipartFormContent.Add(data, "file", "image.jpg");

		using var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
		httpRequest.Content = multipartFormContent;
		if (authToken is not null)
		{
			httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
		}
		var response = await _httpClient.SendAsync(httpRequest);
		return new M33tingClubResponse<T>(response.StatusCode, response.Content);
	}
	
	protected async Task<M33tingClubResponse> Put<TRequest>(string url, TRequest request, string? authToken)
	{
		var data = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

		using (var httpRequest = new HttpRequestMessage(HttpMethod.Put, url))
		{
			httpRequest.Content = data;
			if (authToken is not null)
			{
				httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
			}

			var response = await _httpClient.SendAsync(httpRequest);

			return new M33tingClubResponse(response.StatusCode, response.Content);
		}
	}
	
	protected async Task<M33tingClubResponse> Delete(string url, string? authToken = null)
	{
		using (var httpRequest = new HttpRequestMessage(HttpMethod.Delete, url))
		{
			if (authToken is not null)
			{
				httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
			}

			var response = await _httpClient.SendAsync(httpRequest);

			return new M33tingClubResponse(response.StatusCode, response.Content);
		}
	}

	protected string BuildQuery(List<string>? parameterWithValues)
	{
		if (parameterWithValues is null || !parameterWithValues.Any())
		{
			return "";
		}

		return "?" + string.Join("&", parameterWithValues);
	}
}