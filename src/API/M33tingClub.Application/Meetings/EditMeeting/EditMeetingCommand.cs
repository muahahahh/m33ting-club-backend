using M33tingClub.Application.Utilities;
using MediatR;

namespace M33tingClub.Application.Meetings.EditMeeting;

public record EditMeetingCommand(
    Guid Id, 
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
    string? ConfidentialInfo) : ICommand<Unit>;