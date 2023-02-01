using M33tingClub.Application.Utilities;

namespace M33tingClub.Application.Meetings.AddMeeting;

public record AddMeetingCommand(
    string Name, 
    string Description, 
    int? ParticipantsLimit,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate,
    Guid ImageId,
    string LocationName,
    string LocationDescription,
    double Longitude, 
    double Latitude,
    List<string> Tags,
    bool IsPublic,
    string? ConfidentialInfo
    ) : ICommand<ObjectCreatedResponse>;




