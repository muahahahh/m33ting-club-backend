using M33tingClub.Application.Utilities;
using M33tingClub.Application.Utilities.ImageStorageServiceInterfaces;

namespace M33tingClub.Infrastructure.ImageStorageServiceImplementations;
using Amazon.S3;
using Amazon.S3.Model;

public class BackgroundImageStorageService : ImageStorageService, IBackgroundImageStorageService
{
    public BackgroundImageStorageService(IAmazonS3 amazonS3, string bucketName, string folder) : 
        base(amazonS3, bucketName, folder)
    {
    }
}
