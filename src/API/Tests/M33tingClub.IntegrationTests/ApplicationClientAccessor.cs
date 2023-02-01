using System.Net.Http;
using M33tingClub.IntegrationTests.Utilities;

namespace M33tingClub.IntegrationTests;

internal static class ApplicationClientAccessor
{
    private static readonly TestingWebAppFactory _factory = new();
    
    private static HttpClient? _applicationClient;

    public static HttpClient ApplicationClient
    {
        get
        {
            if (_applicationClient is null)
            {
                _applicationClient = _factory.CreateClient();
            }
            return _applicationClient;
        }
    }
}