namespace M33tingClub.Domain.Utilities.Exceptions;

public class EnvironmentNotSupportedException : Exception
{
    public EnvironmentNotSupportedException(string currentEnvironment)
        : base($"{currentEnvironment} is not supported. " +
               $"Available environments: {string.Join(", ", EnvironmentType.AvailableEnvironments)}")
    {
        
    }
}