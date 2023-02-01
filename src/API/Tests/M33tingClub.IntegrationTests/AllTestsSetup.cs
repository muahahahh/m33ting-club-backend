using System.Threading.Tasks;
using M33tingClub.IntegrationTests.Authentication;
using M33tingClub.IntegrationTests.Utilities.Clients;
using NUnit.Framework;

//it must be in that namespace to run before
//all tests
namespace M33tingClub.IntegrationTests;

[SetUpFixture]
internal class AllTestsSetup
{
    [OneTimeSetUp]
    public async Task BeforeAllTests()
    {
        //This is used to create instance of m33tingClub application
        //Without that there will be no firebase default instance
        var applicationClient = ApplicationClientAccessor.ApplicationClient;
        
        var usersClient = new UsersClient(applicationClient);
        
        await TestAuthCreator.Setup(usersClient);
    }
}