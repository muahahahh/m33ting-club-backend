using System;
using M33tingClub.Domain.Utilities;
using M33tingClub.Web;
using Microsoft.AspNetCore.Mvc.Testing;

namespace M33tingClub.IntegrationTests.Utilities;

internal class TestingWebAppFactory : WebApplicationFactory<Program>
{
    public TestingWebAppFactory()
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", EnvironmentType.Testing);
    }
}