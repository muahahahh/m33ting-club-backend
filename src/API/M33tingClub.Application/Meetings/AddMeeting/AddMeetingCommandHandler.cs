using M33tingClub.Application.Tags;
using M33tingClub.Application.Utilities;
using M33tingClub.Domain.Auth;
using M33tingClub.Domain.Meetings;
using M33tingClub.Domain.Tags;
using M33tingClub.Domain.Users;
using M33tingClub.Domain.Utilities;

namespace M33tingClub.Application.Meetings.AddMeeting;

public class AddMeetingCommandHandler : ICommandHandler<AddMeetingCommand, ObjectCreatedResponse>
{
    private readonly IMeetingRepository _meetingRepository;
    private readonly ITagRepository _tagRepository;
    private readonly IUserContext _userContext;

    public AddMeetingCommandHandler(
        IMeetingRepository meetingRepository,
        ITagRepository tagRepository,
        IUserContext userContext)
    {
        _meetingRepository = meetingRepository;
        _tagRepository = tagRepository;
        _userContext = userContext;
    }
    
    public async Task<ObjectCreatedResponse> Handle(AddMeetingCommand command, CancellationToken cancellationToken)
    {
        var meetingId = MeetingId.CreateNew();
        var userId = UserId.FromGuid(_userContext.UserId);

        var existingTags = 
            (await _tagRepository.GetMany(
                command.Tags.Select(TagName.Create)
                .ToList()))
            .Select(x => x.GetInfo())
            .ToList();

        var newTags = 
            command.Tags.Except(
                    existingTags.Select(x => x.Name.Value))
                .Select(x => TagInfo.Create(TagName.Create(x)))
                .ToList();

        var tagInfos = existingTags.Union(newTags).ToList();

        foreach (var newTag in newTags)
        {
            var tag = Tag.CreateCommunity(newTag.Name);
            await _tagRepository.Add(tag);
        }

        var meeting = Meeting.Create(
            meetingId, 
            command.Name, 
            command.Description, 
            command.ParticipantsLimit,
            command.StartDate, 
            command.EndDate,
            command.ImageId, 
            command.LocationName,
            command.LocationDescription,
            command.Longitude, 
            command.Latitude, 
            tagInfos,
            userId,
            Clock.Now,
            command.IsPublic,
            command.ConfidentialInfo);

        await _meetingRepository.Add(meeting);

        return new ObjectCreatedResponse(meeting.Id.Value);
    }
}
