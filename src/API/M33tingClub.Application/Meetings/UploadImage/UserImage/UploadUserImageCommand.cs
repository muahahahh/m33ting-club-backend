using M33tingClub.Application.Utilities;

namespace M33tingClub.Application.Meetings.UploadImage.UserImage;

public record UploadUserImageCommand(MemoryStream Stream) : ICommand<ObjectCreatedResponse>;
