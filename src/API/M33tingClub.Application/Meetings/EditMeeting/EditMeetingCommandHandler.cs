using M33tingClub.Application.Tags;
using M33tingClub.Application.Utilities;
using M33tingClub.Domain.Auth;
using M33tingClub.Domain.Meetings;
using M33tingClub.Domain.Tags;
using M33tingClub.Domain.Users;
using M33tingClub.Domain.Utilities;
using MediatR;

namespace M33tingClub.Application.Meetings.EditMeeting;

public class EditMeetingCommandHandler : ICommandHandler<EditMeetingCommand, Unit>
{
    private readonly IMeetingRepository _meetingRepository;
    
    private readonly ITagRepository _tagRepository;

    private readonly IUserContext _userContext;

    public EditMeetingCommandHandler(
        IMeetingRepository meetingRepository,
        IUserContext userContext,
        ITagRepository tagRepository)
    {
        _meetingRepository = meetingRepository;
        _userContext = userContext;
        _tagRepository = tagRepository;
    }

    public async Task<Unit> Handle(EditMeetingCommand command, CancellationToken cancellationToken)
    {
        var meeting = await _meetingRepository.GetOrThrow(MeetingId.FromGuid(command.Id));

        var userId = UserId.FromGuid(_userContext.UserId);

        var existingTags = 
            (await _tagRepository.GetMany(
                command.Tags.Select(x => TagName.Create(x))
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

        meeting.Edit(
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

        return await Unit.Task;
    }
}