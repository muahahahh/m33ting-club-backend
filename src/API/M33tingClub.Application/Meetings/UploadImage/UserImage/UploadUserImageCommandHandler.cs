using M33tingClub.Application.Meetings.UploadImage.Rules;
using M33tingClub.Application.Utilities;
using M33tingClub.Application.Utilities.ImageData;
using M33tingClub.Application.Utilities.ImageStorageServiceInterfaces;
using M33tingClub.Domain.Utilities;
using SixLabors.ImageSharp;

namespace M33tingClub.Application.Meetings.UploadImage.UserImage;

public class UploadUserImageCommandHandler : ICommandHandler<UploadUserImageCommand, ObjectCreatedResponse>
{
    private readonly IUserImageStorageService _userImageStorageService;
    
    public UploadUserImageCommandHandler(IUserImageStorageService userImageStorageService)
    {
        _userImageStorageService = userImageStorageService;
    }
    
    public async Task<ObjectCreatedResponse> Handle(UploadUserImageCommand command, CancellationToken cancellationToken)
    {
        var stream = command.Stream;
        
        var widthBig = UserImageConsts.Width;
        var heightBig = UserImageConsts.Height;

        RuleChecker.CheckRule(new ImageCannotBeTooLargeRule(stream.Length));

        stream.Position = 0;
        var imageInfo = await Image.IdentifyAsync(stream, cancellationToken);
        var imageIsInCorrectFormat = imageInfo is not null;
        
        RuleChecker.CheckRule(new ImageMustBeInCorrectFormatRule(imageIsInCorrectFormat));
        RuleChecker.CheckRule(new UserImageResolutionMustBeCorrectRule(imageInfo!.Width, imageInfo.Height));
        RuleChecker.CheckRule(new UserImageAspectRatioMustBeCorrectRule(imageInfo.Width, imageInfo.Height));
        
        stream.Position = 0;
        var image = await Image.LoadAsync(stream, cancellationToken);
        
        var id = Guid.NewGuid();
        
        var outputStreamBig = new MemoryStream();
        var imageBig = ImageTools.CropAndResize(image, widthBig, heightBig);
        await imageBig.SaveAsJpegAsync(outputStreamBig, cancellationToken);

        await _userImageStorageService.PutObject($"{id}.jpg", outputStreamBig);

        return new ObjectCreatedResponse(id);
    }
}

