using System.IO;
using M33tingClub.Application.Utilities;
using M33tingClub.Domain.Utilities.Exceptions;
using M33tingClub.UnitTests.Utilities;
using NUnit.Framework;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace M33tingClub.UnitTests.Tools.ImageToolsTests;

[TestFixture]
// TODO: Fix that using rule check
public class ValidateResolutionTests : TestBase
{
    /*[Test]
    public void GivenFittingImage_WhenCheckedResolutionValidity_ThenExceptionIsNotThrown()
    {
        var stream = new MemoryStream();
        var image = new Image<Rgba32>(780, 975, Rgba32.ParseHex("#FF0000"));
        image.SaveAsJpeg(stream);

        stream.Position = 0;
        var imageInfo = Image.Identify(stream);
        
        Assert.DoesNotThrow(() => ImageTools.ValidateMinimalResolution(imageInfo, 780, 975));
    }
    
    [Test]
    public void GivenBigImage_WhenCheckedResolutionValidity_ThenExceptionIsNotThrown()
    {
        var stream = new MemoryStream();
        var image = new Image<Rgba32>(1000, 2000, Rgba32.ParseHex("#FF0000"));
        image.SaveAsJpeg(stream);

        stream.Position = 0;
        var imageInfo = Image.Identify(stream);
        
        Assert.DoesNotThrow(() => ImageTools.ValidateMinimalResolution(imageInfo, 780, 975));
    }
    
    [Test]
    public void GivenTooSmallImage_WhenCheckedResolutionValidity_ThenExceptionIsThrown()
    {
        var stream = new MemoryStream();
        var image = new Image<Rgba32>(780, 500, Rgba32.ParseHex("#FF0000"));
        image.SaveAsJpeg(stream);

        stream.Position = 0;
        var imageInfo = Image.Identify(stream);
        
        Assert.Throws<ImageResolutionTooLowException>(() => ImageTools.ValidateMinimalResolution(imageInfo, 780, 975));
    }*/
}