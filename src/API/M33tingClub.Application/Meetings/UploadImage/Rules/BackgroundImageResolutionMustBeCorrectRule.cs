using M33tingClub.Application.Utilities.ImageData;
using M33tingClub.Domain.Utilities;

namespace M33tingClub.Application.Meetings.UploadImage.Rules;

// TODO: Check in integration tests and unit tests
public class BackgroundImageResolutionMustBeCorrectRule : IRule
{
    private const int MinWidth = BackgroundImageBigConsts.Width;
    private const int MinHeight = BackgroundImageBigConsts.Height;

    private readonly int _imageWidth;
    private readonly int _imageHeight;
    
    public BackgroundImageResolutionMustBeCorrectRule(
        int imageWidth, 
        int imageHeight)
    {
        _imageWidth = imageWidth;
        _imageHeight = imageHeight;
    }

    public bool IsBroken()
        => _imageWidth < MinWidth || _imageHeight < MinHeight;

    public string Message => $"Image must have at least {MinWidth}px width and {MinHeight}px height.";
    
    public RuleExceptionKind Kind => RuleExceptionKind.BadRequest;
}