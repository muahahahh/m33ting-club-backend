using M33tingClub.Domain.Utilities.Exceptions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace M33tingClub.Application.Utilities;

public static class ImageTools
{
    public static Image CropAndResize(Image image, int targetWidth, int targetHeight)
    {
        var size = image.Size();

        var x = 0;
        var y = 0;
        if (size.Width > size.Height)
        {
            x = size.Width / 2 - targetWidth / 2;
        }
        
        if (size.Width / size.Height != targetWidth / targetHeight)
        {
            image.Mutate(img => img.Crop(Rectangle.FromLTRB(x, y, x + targetWidth, y + targetHeight)));
        }
        var processedImage = image.Clone(img => img.Resize(targetWidth, targetHeight));
        return processedImage;
    }
}