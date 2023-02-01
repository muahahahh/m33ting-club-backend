using M33tingClub.Domain.Utilities;

namespace M33tingClub.Web;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder().Build().Run();
    }

    private static IHostBuilder CreateHostBuilder()
        => Host.CreateDefaultBuilder()
            .ConfigureLogging(x =>
            {
                x.ClearProviders();
                x.AddConsole();
            })
               .ConfigureWebHostDefaults(x => x.UseStartup<Startup>())
               .ConfigureAppConfiguration(
                   builder =>
                   {
                       builder.Sources.Clear();
                       builder
                           .AddJsonFile("appsettings.json", false)
                           .AddJsonFile($"appsettings.{EnvironmentType.CurrentEnvironment}.json", true)
                           .AddEnvironmentVariables("M33tingClub:") // For Linux
                           .AddEnvironmentVariables("M33tingClub__"); // For Windows
                   });
}