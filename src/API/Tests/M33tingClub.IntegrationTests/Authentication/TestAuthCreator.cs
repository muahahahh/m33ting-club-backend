using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using M33tingClub.IntegrationTests.Utilities;
using M33tingClub.IntegrationTests.Utilities.Clients;
using M33tingClub.Web;
using Newtonsoft.Json;

namespace M33tingClub.IntegrationTests.Authentication;

internal static class TestAuthCreator
{
    internal const string FirebaseCredentialsPath = "Authentication/firebase-configuration.json";
    
    public static async Task Setup(UsersClient usersClient)
    {
        var configuration = TestConfigurationProvider.Get();
        var meetingClubConfiguration = new M33tingClubConfiguration(configuration);
        
        var firebaseConfiguration = ReadFirebaseConfiguration();
        var firebaseClient = new FirebaseClient(firebaseConfiguration.ApiKey);

        await firebaseClient.RemoveAllUsers();

        await DatabaseCleaner.Clean(meetingClubConfiguration.DatabaseConnectionString, true);
        
        var authUsers = await AuthenticateUsers(firebaseClient, usersClient);
        
        TestAuthUsersProvider.SetUsers(authUsers);
    }
    
    internal static FirebaseConfiguration ReadFirebaseConfiguration()
    {
        using (var reader = new StreamReader(FirebaseCredentialsPath))
        {
            var json = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<FirebaseConfiguration>(json);
        }
    }
    
    private static async Task<Dictionary<string, TestAuthUser>> AuthenticateUsers(
        FirebaseClient firebaseClient,
        UsersClient usersClient)
    {
        var usersCredentials = AuthUserConsts.GetUsersCredentials();

        var testAuthUsers = new Dictionary<string, TestAuthUser>();
        foreach (var userCredentials in usersCredentials)
        {
            var testAuthUser = await firebaseClient.AddToFirebaseAndAuthorize(
                userCredentials.Value,
                usersClient);
            
            testAuthUsers.Add(userCredentials.Key, testAuthUser);
        }

        return testAuthUsers;
    }
}