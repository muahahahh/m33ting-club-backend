using System.Threading.Tasks;
using Amazon.S3;
using M33tingClub.Infrastructure.ImageStorageServiceImplementations;

namespace M33tingClub.IntegrationTests.Utilities;

internal static class FileStorageCleaner
{
    private static readonly IAmazonS3 AmazonS3 = new AmazonS3Client();
    
    public static async Task Clean(string bucket, string folder)
    {
        var service = new BackgroundImageStorageService(AmazonS3, bucket, folder);

        var keysToDelete = service.GetAllObjectKeys();
        await service.DeleteObjects(keysToDelete);
    }
}