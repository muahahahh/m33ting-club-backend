using M33tingClub.Domain.Utilities;

namespace M33tingClub.Application.Meetings.UploadImage.Rules;

// TODO: Check in integration test
public class ImageCannotBeTooLargeRule : IRule
{
    private const int MaxAllowedSizeBytes = 5242880;

    private readonly long _fileSizeInBytes;
    
    public ImageCannotBeTooLargeRule(long fileSizeInBytes)
    {
        _fileSizeInBytes = fileSizeInBytes;
    }

    public bool IsBroken()
        => _fileSizeInBytes > MaxAllowedSizeBytes;

    public string Message => $"Image is too large, max accepted size: {MaxAllowedSizeBytes} bytes.";
    
    public RuleExceptionKind Kind => RuleExceptionKind.BadRequest;
}