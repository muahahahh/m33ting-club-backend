using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FirebaseAdmin.Auth;
using M33tingClub.Application.Users.FinishUserSignUp;
using M33tingClub.IntegrationTests.Utilities.Clients;

namespace M33tingClub.IntegrationTests.Authentication;

internal class FirebaseClient
{
    private readonly HttpClient _httpClient;

    private readonly string _apiKey;

    internal FirebaseClient(string apiKey)
    {
        _httpClient = new HttpClient();
        _apiKey = apiKey;
    }

    internal async Task RemoveAllUsers()
    {
        var userUids = new List<string>(); 
        var pagedEnumerable = FirebaseAuth.DefaultInstance.ListUsersAsync(null);
        var responses = pagedEnumerable.AsRawResponses().GetAsyncEnumerator();
        while (await responses.MoveNextAsync())
        {
            ExportedUserRecords response = responses.Current;

            if (response.Users is null)
            {
                return;
            }
            
            foreach (ExportedUserRecord user in response.Users)
            {
                userUids.Add(user.Uid);
            }
        }

        await FirebaseAuth.DefaultInstance.DeleteUsersAsync(userUids);
    }
    
    internal async Task<TestAuthUser> AddToFirebaseAndAuthorize(
        TestUserCredentials testUserCredentials,
        UsersClient usersClient)
    {
        UserRecordArgs args = new UserRecordArgs
        {
            Email = testUserCredentials.Email,
            PhoneNumber = testUserCredentials.PhoneNumber,
            EmailVerified = false,
            Password = testUserCredentials.Password,
            DisplayName = testUserCredentials.Name,
            Disabled = false,
        };
        
        UserRecord userRecord = await FirebaseAuth.DefaultInstance.CreateUserAsync(args);
        
        var firstLoginToken = await GetToken(testUserCredentials);

        var finishUserSignUpCommand = new SignUpCommand(
            testUserCredentials.Name,
            testUserCredentials.Birthday,
            testUserCredentials.Gender,
            testUserCredentials.ImageId);
        
        await usersClient.FinishSignUp(finishUserSignUpCommand, firstLoginToken);

        var completelyRegisteredToken = await GetToken(testUserCredentials);
        
        var user = (await usersClient.Me(completelyRegisteredToken)).Content;
        
        return new TestAuthUser(
            user!.Id, 
            user.FirebaseUid, 
            user.Name,
            user.PhoneNumber,
            user.Birthday, 
            user.Gender,  
            user.ImageId!.Value,
            completelyRegisteredToken);
    }

    private async Task<string> GetToken(TestUserCredentials testUserCredentials)
    {
        var url = $"https://www.googleapis.com/identitytoolkit/v3/relyingparty/verifyPassword?key={_apiKey}";

        var body = new
        {
            email = testUserCredentials.Email,
            password = testUserCredentials.Password,
            returnSecureToken = true
        };
        
        HttpContent data = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PostAsync(url, data);

        var result = await response.Content.ReadFromJsonAsync<AuthResult>();

        return result!.IdToken;
    }
    
    private class AuthResult
    {
        public string Kind { get; set; }
    
        public string LocalId { get; set; }
    
        public string Email { get; set; }
    
        public string DisplayName { get; set; }
        
        public string IdToken { get; set; }
    
        public bool Registered { get; set; }
    }
}