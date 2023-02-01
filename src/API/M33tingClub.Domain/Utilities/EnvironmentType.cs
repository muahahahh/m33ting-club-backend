using M33tingClub.Domain.Utilities.Exceptions;

namespace M33tingClub.Domain.Utilities;

public static class EnvironmentType
{
    public static string Development => nameof(Development);

    public static string Testing => nameof(Testing);

    public static string Staging => nameof(Staging);

    public static string Production => nameof(Production);

    public static List<string> AvailableEnvironments => new()
    {
        Development,
        Testing,
        Staging,
        Production
    };
    
    private static string? _currentEnvironment;

    public static string CurrentEnvironment
    {
        get
        {
            if (_currentEnvironment is null)
            {
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                if (AvailableEnvironments.Contains(environment))
                {
                    _currentEnvironment = environment;
                    return _currentEnvironment;
                }

                throw new EnvironmentNotSupportedException(environment);
            }
            
            return _currentEnvironment!;
        }
    }
}