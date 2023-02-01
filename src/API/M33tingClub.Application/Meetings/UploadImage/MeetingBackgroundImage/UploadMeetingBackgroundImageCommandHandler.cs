
using M33tingClub.Application.Meetings.UploadImage.Rules;
using M33tingClub.Application.Utilities;
using M33tingClub.Application.Utilities.ImageData;
using M33tingClub.Application.Utilities.ImageStorageServiceInterfaces;
using M33tingClub.Domain.Utilities;
using SixLabors.ImageSharp;

namespace M33tingClub.Application.Meetings.UploadImage.MeetingBackgroundImage;

public class UploadMeetingBackgroundImageCommandHandler : ICommandHandler<UploadMeetingBackgroundImageCommand, ObjectCreatedResponse>
{
    private readonly IBackgroundImageStorageService _backgroundImageStorageService;
    
    public UploadMeetingBackgroundImageCommandHandler(IBackgroundImageStorageService backgroundImageStorageService)
    {
        _backgroundImageStorageService = backgroundImageStorageService;
    }
    
    public async Task<ObjectCreatedResponse> Handle(UploadMeetingBackgroundImageCommand command, CancellationToken cancellationToken)
    {
        var stream = command.Stream;
        
        var widthBig = BackgroundImageBigConsts.Width;
        var heightBig = BackgroundImageBigConsts.Height;
        var widthSmall = BackgroundImageSmallConsts.Width;
        var heightSmall = BackgroundImageSmallConsts.Height;
        
        RuleChecker.CheckRule(new ImageCannotBeTooLargeRule(stream.Length));

        stream.Position = 0;
        var imageInfo = await Image.IdentifyAsync(stream, cancellationToken);
        var imageIsInCorrectFormat = imageInfo is not null;
        
        RuleChecker.CheckRule(new ImageMustBeInCorrectFormatRule(imageIsInCorrectFormat));
        RuleChecker.CheckRule(new BackgroundImageResolutionMustBeCorrectRule(imageInfo!.Width, imageInfo.Height));
        RuleChecker.CheckRule(new BackgroundImageAspectRatioMustBeCorrectRule(imageInfo.Width, imageInfo.Height));
        
        stream.Position = 0;
        var image = await Image.LoadAsync(stream, cancellationToken);
        
        var id = Guid.NewGuid();
        
        var outputStreamBig = new MemoryStream();
        var imageBig = ImageTools.CropAndResize(image, widthBig, heightBig);
        await imageBig.SaveAsJpegAsync(outputStreamBig, cancellationToken);

        await _backgroundImageStorageService.PutObject($"{id}-big.jpg", outputStreamBig);
        
        var outputStreamSmall = new MemoryStream();
        var imageSmall = ImageTools.CropAndResize(image, widthSmall, heightSmall);
        await imageSmall.SaveAsJpegAsync(outputStreamSmall, cancellationToken);

        await _backgroundImageStorageService.PutObject($"{id}-small.jpg", outputStreamSmall);
        
        return new ObjectCreatedResponse(id);
    }
}

