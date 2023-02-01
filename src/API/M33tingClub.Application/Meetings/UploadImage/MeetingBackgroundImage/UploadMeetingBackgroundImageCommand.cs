using M33tingClub.Application.Utilities;

namespace M33tingClub.Application.Meetings.UploadImage.MeetingBackgroundImage;

public record UploadMeetingBackgroundImageCommand(MemoryStream Stream) : ICommand<ObjectCreatedResponse>;
