namespace M33tingClub.Web;

public class M33tingClubConfiguration
{
    public string DatabaseConnectionString => _configuration["Database:ConnectionString"];
    
    public string FirebaseProjectId => _configuration["Firebase:ProjectId"];

    public string FirebaseCredentialsPath => _configuration["Firebase:CredentialsPath"];

    public string S3BackgroundImageBucketName => _configuration["S3:Backgrounds:ImageBucketName"];
    public string S3BackgroundImageFolderName => _configuration["S3:Backgrounds:Folder"];
    
    public string S3UserImageBucketName => _configuration["S3:UserImages:ImageBucketName"];
    public string S3UserImageFolderName => _configuration["S3:UserImages:Folder"];

    private readonly IConfiguration _configuration;
    
    public M33tingClubConfiguration(IConfiguration configuration)
    {
        _configuration = configuration;
    }
}