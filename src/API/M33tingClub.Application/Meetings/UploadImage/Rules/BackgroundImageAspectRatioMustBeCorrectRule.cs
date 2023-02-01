using M33tingClub.Domain.Utilities;

namespace M33tingClub.Application.Meetings.UploadImage.Rules;

public class BackgroundImageAspectRatioMustBeCorrectRule : IRule
{
    private const int AspectRatioWidth = 1;
    private const int AspectRatioHeight = 1;

    private readonly int _imageWidth;
    private readonly int _imageHeight;
    
    public BackgroundImageAspectRatioMustBeCorrectRule(
        int imageWidth, 
        int imageHeight)
    {
        _imageWidth = imageWidth;
        _imageHeight = imageHeight;
    }

    public bool IsBroken()
        => (double)_imageWidth / _imageHeight != (double)AspectRatioWidth / AspectRatioHeight;

    public string Message => $"Image aspect ratio must be {AspectRatioWidth}:{AspectRatioHeight}.";
    
    public RuleExceptionKind Kind => RuleExceptionKind.BadRequest;
}