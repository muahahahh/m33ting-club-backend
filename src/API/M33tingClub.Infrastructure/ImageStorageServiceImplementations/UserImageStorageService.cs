
using M33tingClub.Application.Utilities.ImageStorageServiceInterfaces;

namespace M33tingClub.Infrastructure.ImageStorageServiceImplementations;
using Amazon.S3;
using Amazon.S3.Model;

public class UserImageStorageService : ImageStorageService, IUserImageStorageService
{
    public UserImageStorageService(IAmazonS3 amazonS3, string bucketName, string folder) : 
        base(amazonS3, bucketName, folder)
    {
    }
}
