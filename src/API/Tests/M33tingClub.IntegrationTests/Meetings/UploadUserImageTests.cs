
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using M33tingClub.Application.Utilities.ImageData;
using M33tingClub.IntegrationTests.Authentication;
using NUnit.Framework;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using M33tingClub.IntegrationTests.Utilities;

namespace M33tingClub.IntegrationTests.Meetings;

[TestFixture]
internal class UploadUserImageTests : TestBase
{
    [Test]
    public async Task WhenUploadPhotoFillingCriteria_ThenPhotoIsUploaded()
    {
        // Given
        var maxUser = TestAuthUsersProvider.MaxVerstappen;
        
        var width = UserImageConsts.Width;
        var height = UserImageConsts.Height;

        var image = new Image<Rgba32>(width, height, Rgba32.ParseHex("#FF0000"));
        var stream = new MemoryStream();
        await image.SaveAsJpegAsync(stream);

        stream.Position = 0;
        var imageId = (await UsersClient.UploadUserImage(stream, maxUser)).Content.Id;

        var bucketName = Configuration.S3UserImageBucketName;
        var folder = Configuration.S3UserImageFolderName;
        
        var urlImage = $"https://{bucketName}.s3.eu-central-1.amazonaws.com/{folder}/{imageId}.jpg";

        var webClient = new HttpClient();
        
        var streamImage = webClient.GetStreamAsync(urlImage).Result;
        var uploadedImage = await Image.LoadAsync(streamImage);
        
        uploadedImage.Width.Should().Be(width);
        uploadedImage.Height.Should().Be(height);
    }
    
    [Test]
    public async Task WhenUploadPhotoWithWidthTooLow_ThenPhotoIsNotUploaded()
    {
        // Given
        var maxUser = TestAuthUsersProvider.MaxVerstappen;
        
        var width = UserImageConsts.Width;
        var height = UserImageConsts.Height;

        var image = new Image<Rgba32>(width-1, height, Rgba32.ParseHex("#FF0000"));
        var stream = new MemoryStream();
        await image.SaveAsJpegAsync(stream);

        stream.Position = 0;
        // When
        var response = await UsersClient.UploadUserImage(stream, maxUser);
        
        // Then
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Test]
    public async Task WhenUploadPhotoWithHeightTooLow_ThenPhotoIsNotUploaded()
    {
        // Given
        var maxUser = TestAuthUsersProvider.MaxVerstappen;
        
        var width = UserImageConsts.Width;
        var height = UserImageConsts.Height;

        var image = new Image<Rgba32>(width, height-1, Rgba32.ParseHex("#FF0000"));
        var stream = new MemoryStream();
        await image.SaveAsJpegAsync(stream);

        stream.Position = 0;
        // When
        var response = (await UsersClient.UploadUserImage(stream, maxUser));
        
        // Then
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Test]
    public async Task WhenUploadPhotoWithWrongAspectRatio_ThenPhotoIsNotUploaded()
    {
        // Given
        var maxUser = TestAuthUsersProvider.MaxVerstappen;
        
        var width = UserImageConsts.Width + 1;
        var height = UserImageConsts.Height;

        var image = new Image<Rgba32>(width, height, Rgba32.ParseHex("#FF0000"));
        var stream = new MemoryStream();
        await image.SaveAsJpegAsync(stream);

        stream.Position = 0;
        // When
        var response = (await UsersClient.UploadUserImage(stream, maxUser));
        
        // Then
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}