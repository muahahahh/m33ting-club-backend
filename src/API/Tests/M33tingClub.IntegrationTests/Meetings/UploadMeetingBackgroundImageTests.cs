
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
internal class UploadMeetingBackgroundImageTests : TestBase
{
    [Test]
    public async Task WhenUploadPhoto_ThenPhotoIsUploaded()
    {
        
        // Given
        var maxUser = TestAuthUsersProvider.MaxVerstappen;
        
        var widthBig = BackgroundImageBigConsts.Width;
        var heightBig = BackgroundImageBigConsts.Height;
        var widthSmall = BackgroundImageSmallConsts.Width;
        var heightSmall = BackgroundImageSmallConsts.Height;
        
        var image = new Image<Rgba32>(1000, 1000, Rgba32.ParseHex("#FF0000"));
        var stream = new MemoryStream();
        await image.SaveAsJpegAsync(stream);

        stream.Position = 0;
        var imageId = (await MeetingsClient.UploadBackgroundImage(stream, maxUser)).Content.Id;

        var bucketName = Configuration.S3BackgroundImageBucketName;
        var folder = Configuration.S3BackgroundImageFolderName;
        
        var urlImageBig = $"https://{bucketName}.s3.eu-central-1.amazonaws.com/{folder}/{imageId}-big.jpg";
        var urlImageSmall = $"https://{bucketName}.s3.eu-central-1.amazonaws.com/{folder}/{imageId}-small.jpg";
        
        var webClient = new HttpClient();
        
        var streamImageBig = webClient.GetStreamAsync(urlImageBig).Result;
        var uploadedImageBig = await Image.LoadAsync(streamImageBig);
        
        uploadedImageBig.Width.Should().Be(widthBig);
        uploadedImageBig.Height.Should().Be(heightBig);
        
        var streamImageSmall = webClient.GetStreamAsync(urlImageSmall).Result;
        var uploadedImageSmall = await Image.LoadAsync(streamImageSmall);
        
        uploadedImageSmall.Width.Should().Be(widthSmall);
        uploadedImageSmall.Height.Should().Be(heightSmall);
    }
    
    [Test]
    public async Task WhenUploadPhotoWithWidthTooLow_ThenPhotoIsNotUploaded()
    {
        // Given
        var maxUser = TestAuthUsersProvider.MaxVerstappen;
        
        var width = BackgroundImageBigConsts.Width;
        var height = BackgroundImageBigConsts.Height;

        var image = new Image<Rgba32>(width-1, height, Rgba32.ParseHex("#FF0000"));
        var stream = new MemoryStream();
        await image.SaveAsJpegAsync(stream);

        stream.Position = 0;
        // When
        var response = (await MeetingsClient.UploadBackgroundImage(stream, maxUser));
        
        // Then
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Test]
    public async Task WhenUploadPhotoWithHeightTooLow_ThenPhotoIsNotUploaded()
    {
        // Given
        var maxUser = TestAuthUsersProvider.MaxVerstappen;
        
        var width = BackgroundImageBigConsts.Width;
        var height = BackgroundImageBigConsts.Height;

        var image = new Image<Rgba32>(width, height-1, Rgba32.ParseHex("#FF0000"));
        var stream = new MemoryStream();
        await image.SaveAsJpegAsync(stream);

        stream.Position = 0;
        // When
        var response = (await MeetingsClient.UploadBackgroundImage(stream, maxUser));
        
        // Then
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Test]
    public async Task WhenUploadPhotoWithWrongAspectRatio_ThenPhotoIsNotUploaded()
    {
        // Given
        var maxUser = TestAuthUsersProvider.MaxVerstappen;
        
        var width = BackgroundImageBigConsts.Width + 1;
        var height = BackgroundImageBigConsts.Height;

        var image = new Image<Rgba32>(width, height, Rgba32.ParseHex("#FF0000"));
        var stream = new MemoryStream();
        await image.SaveAsJpegAsync(stream);

        stream.Position = 0;
        // When
        var response = (await MeetingsClient.UploadBackgroundImage(stream, maxUser));
        
        // Then
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}