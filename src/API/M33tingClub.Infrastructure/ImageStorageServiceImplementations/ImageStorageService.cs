namespace M33tingClub.Infrastructure.ImageStorageServiceImplementations;

using M33tingClub.Application.Utilities;
using M33tingClub.Application.Utilities.ImageStorageServiceInterfaces;
using Amazon.S3;
using Amazon.S3.Model;

public class ImageStorageService : IImageStorageService
{
    private readonly IAmazonS3 _amazonS3;
    private readonly string _bucketName;
    private readonly string _folder;

    protected ImageStorageService(IAmazonS3 amazonS3, string bucketName, string folder)
    {
        _amazonS3 = amazonS3;
        _bucketName = bucketName;
        _folder = folder;
    }

    public async Task PutObject(string name,  MemoryStream stream)
    {
        var request = new PutObjectRequest
        {
            Key = $"{_folder}/{name}",
            InputStream = stream,
            BucketName = _bucketName
        };
        await _amazonS3.PutObjectAsync(request);
    }

    public List<string> GetAllObjectKeys()
    {
        var results = new List<string>();
        
        ListObjectsRequest request = new ListObjectsRequest
        {
            BucketName = _bucketName,
            Prefix = _folder,
        };

        while (true){
            var response = _amazonS3.ListObjectsAsync(request);
            foreach (S3Object obj in response.Result.S3Objects)
            {
                results.Add(obj.Key);
            }

            if (response.Result.IsTruncated)
            {
                request.Marker = response.Result.NextMarker;
            }
            else
            {
                break;
            }
        }
        return results;
    }

    public async Task DeleteObjects(List<string> keys)
    {
        if (keys.Any())
        {
            var request = new DeleteObjectsRequest
            {
                BucketName = _bucketName
            };

            foreach (var key in keys)
            {
                request.AddKey(key, null);
            }

            await _amazonS3.DeleteObjectsAsync(request);
        }
    }
}