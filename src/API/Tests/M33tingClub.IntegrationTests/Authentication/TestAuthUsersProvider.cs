using System.Collections.Generic;

namespace M33tingClub.IntegrationTests.Authentication;

internal static class TestAuthUsersProvider
{
    public static TestAuthUser LandoNorris { get; private set; }
    
    public static TestAuthUser MaxVerstappen { get; private set; }

    public static TestAuthUser SergioPerez { get; private set; }

    public static void SetUsers(
        Dictionary<string, TestAuthUser> authUsers)
    {
        LandoNorris = authUsers[AuthUserConsts.LandoNorris];
        MaxVerstappen = authUsers[AuthUserConsts.MaxVerstappen];
        SergioPerez = authUsers[AuthUserConsts.SergioPerez];
    }
}