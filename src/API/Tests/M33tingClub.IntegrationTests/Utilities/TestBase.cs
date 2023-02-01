using System.Net.Http;
using System.Threading.Tasks;
using M33tingClub.IntegrationTests.Authentication;
using M33tingClub.IntegrationTests.Utilities.Clients;
using M33tingClub.Web;
using NUnit.Framework;

namespace M33tingClub.IntegrationTests.Utilities;

internal abstract class TestBase
{
    private HttpClient Client => ApplicationClientAccessor.ApplicationClient;

    private MeetingsClient? _meetingClient;

    protected MeetingsClient MeetingsClient
    {
        get
        {
            if (_meetingClient is null)
            {
                _meetingClient = new MeetingsClient(Client);
            }

            return _meetingClient;
        }
    }

    private TagsClient? _tagsClient;

    protected TagsClient TagsClient
    {
        get
        {
            if (_tagsClient is null) _tagsClient = new TagsClient(Client);

            return _tagsClient;
        }
    }
    
    private UsersClient? _usersClient;

    protected UsersClient UsersClient
    {
        get
        {
            if (_usersClient is null) _usersClient = new UsersClient(Client);

            return _usersClient;
        }
    }
    
    private MeetingApplicationsClient? _applicationsClient;

    protected MeetingApplicationsClient MeetingApplicationsClient
    {
        get
        {
            if (_applicationsClient is null) _applicationsClient = new MeetingApplicationsClient(Client);

            return _applicationsClient;
        }
    }

    private M33tingClubConfiguration? _configuration;

    protected M33tingClubConfiguration Configuration
    {
        get
        {
            if (_configuration is null)
            {
                var configuration = TestConfigurationProvider.Get();

                _configuration = new M33tingClubConfiguration(configuration);
            }

            return _configuration;
        }
    }

    [SetUp]
    protected async Task BeforeEachTest()
    {
        await DatabaseCleaner.Clean(Configuration.DatabaseConnectionString);
    }

    [OneTimeTearDown]
    protected async Task AfterAllTests()
    {
        await FileStorageCleaner.Clean(Configuration.S3BackgroundImageBucketName, Configuration.S3BackgroundImageFolderName);
    }
}