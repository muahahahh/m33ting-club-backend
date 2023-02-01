using System.Net;
using System.Net.Http;

namespace M33tingClub.IntegrationTests.Utilities.Clients;

internal static class HttpStatusCodeExtensions
{
    internal static bool IsSuccessful(this HttpStatusCode httpStatusCode)
        => new HttpResponseMessage(httpStatusCode).IsSuccessStatusCode;
}