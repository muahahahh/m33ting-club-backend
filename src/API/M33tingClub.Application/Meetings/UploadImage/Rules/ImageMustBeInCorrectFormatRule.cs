using M33tingClub.Domain.Utilities;

namespace M33tingClub.Application.Meetings.UploadImage.Rules;

// TODO: Check in integration test
public class ImageMustBeInCorrectFormatRule : IRule
{
    private readonly bool _isInCorrectFormat;
    
    public ImageMustBeInCorrectFormatRule(bool isInCorrectFormat)
    {
        _isInCorrectFormat = isInCorrectFormat;
    }

    public bool IsBroken() => !_isInCorrectFormat;

    public string Message => "Image is not in accepted format.";
    
    public RuleExceptionKind Kind => RuleExceptionKind.BadRequest;
}