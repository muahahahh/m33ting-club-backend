using System;
using System.Collections.Generic;

namespace M33tingClub.IntegrationTests.Authentication;

internal static class AuthUserConsts
{
    public static string LandoNorris => "Lando Norris";

    public static string MaxVerstappen => "Max Verstappen";

    public static string SergioPerez => "Sergio Perez";
    
    internal static Dictionary<string, TestUserCredentials> GetUsersCredentials()
    {
        var landoNorrisCredentials = new TestUserCredentials(
            "lando_norris@meet.com",
            "lando_norris",
            "+48111111111",
            "Lando Norris",
            new DateTime(1999, 11, 13, 0, 0, 0, DateTimeKind.Utc),
            "Male",
            Guid.Parse("0FF69A3F-F1FC-4981-8C20-F6C7B0BB28DE"));

        var maxVerstappenCredentials = new TestUserCredentials(
            "max_verstappen@meet.com",
            "max_verstappen",
            "+48222222222",
            "Max Vestappen",
            new DateTime(1997, 9, 30, 0, 0, 0, DateTimeKind.Utc),
            "Male",
            Guid.Parse("C5253AAA-2049-406E-901A-21C2A51554B7"));

        var sergioPerezCredentials = new TestUserCredentials(
            "sergio_perez@meet.com",
            "sergio_perez",
            "+48333333333",
            "Sergio Perez",
            new DateTime(1990, 1, 26, 0, 0, 0, DateTimeKind.Utc),
            "Male",
            Guid.Parse("E94102FF-B79D-4590-97F4-837B1C52AF6D"));

        return new Dictionary<string, TestUserCredentials>
        {
            { LandoNorris, landoNorrisCredentials },
            { MaxVerstappen, maxVerstappenCredentials },
            { SergioPerez, sergioPerezCredentials }
        };
    }
}