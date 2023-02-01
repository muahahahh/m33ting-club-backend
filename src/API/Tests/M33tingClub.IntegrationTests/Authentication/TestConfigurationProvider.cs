using M33tingClub.Domain.Utilities;
using Microsoft.Extensions.Configuration;

namespace M33tingClub.IntegrationTests.Authentication;

internal static class TestConfigurationProvider
{
    public static IConfigurationRoot Get()
        => new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false)
            .AddJsonFile($"appsettings.{EnvironmentType.Testing}.json", true)
            .AddEnvironmentVariables("M33tingClub__")
            .Build();    
}