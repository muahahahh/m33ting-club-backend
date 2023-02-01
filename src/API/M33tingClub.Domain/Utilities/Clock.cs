namespace M33tingClub.Domain.Utilities;

public static class Clock
{
    private static DateTimeOffset? _customDate;

    public static DateTimeOffset Now => _customDate ?? DateTimeOffset.UtcNow;

    public static void Set(DateTimeOffset customDate)
    {
        //TODO: Fix this, since is not working
        /*if (EnvironmentType.CurrentEnvironment != EnvironmentType.Testing)
        {
            throw new ClockSetNotSupportedOutOfTestingEnvironmentException(EnvironmentType.CurrentEnvironment);
        }*/

        _customDate = customDate;
    }
    
    public static void Reset() => _customDate = null;
}