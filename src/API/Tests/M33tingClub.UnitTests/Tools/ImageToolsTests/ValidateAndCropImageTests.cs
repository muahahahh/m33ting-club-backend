using M33tingClub.Application.Utilities;
using M33tingClub.UnitTests.Utilities;
using NUnit.Framework;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace M33tingClub.UnitTests.Tools.ImageToolsTests;

[TestFixture]
public class ValidateAndCropImageTests : TestBase
{
    [Test]
    [TestCase(780, 1000, 780, 975)]
    [TestCase(1000, 1000, 780, 975)]
    [TestCase(780, 975, 780, 975)]
    public void GivenImage_WhenResizedAndCropped_ThenImageOfRespectedSizeIsReturned(
        int startWidth,
        int startHeight,
        int targetWidth,
        int targetHeight)
    {
        var image = new Image<Rgba32>(startWidth, startHeight, Rgba32.ParseHex("#FF0000"));
        
        var resizedImage = ImageTools.CropAndResize(image, targetWidth, targetHeight);

        Assert.AreEqual(resizedImage.Width, targetWidth);
        Assert.AreEqual(resizedImage.Height, targetHeight);
    }
}