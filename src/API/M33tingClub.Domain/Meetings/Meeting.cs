using System.Linq.Expressions;
using M33tingClub.Domain.Meetings.DomainEvents;
using M33tingClub.Domain.Meetings.Rules;
using M33tingClub.Domain.Tags;
using M33tingClub.Domain.Users;
using M33tingClub.Domain.Utilities;

namespace M33tingClub.Domain.Meetings;

public class Meeting : Entity
{
    public MeetingId Id { get; private set; }
    
    private string _name;

    private string? _description;

    private int? _participantsLimit;
    
    private TimeRange _timeRange;
    
    private Guid _imageId;

    private Location _location;

    private List<TagName> _tagNames = new();

    private MeetingStatus _status;

    private List<Participant> _participants = new();

    private List<MeetingApplication> _applications = new();

    private bool _isPublic;

    private string? _confidentialInfo;

    private Meeting()
    {
        // For EF core
    }

    private Meeting(
        MeetingId id, 
        string name, 
        string description, 
        int? participantsLimit,
        DateTimeOffset startDate,
        DateTimeOffset endDate,
        Guid picture, 
        string locationName,
        string locationDescription,
        double longitude, 
        double latitude,
        List<TagInfo> tagInfos,
        UserId ownerId,
        DateTimeOffset currentDate,
        bool isPublic,
        string? confidentialInfo)
    {
        CheckRule(new NumberOfTagsShouldBeBetweenOneAndTenRule(tagInfos));
        CheckRule(new StartDateOfMeetingCannotBeInThePastRule(startDate, currentDate));
        CheckRule(new MeetingShouldHaveAtLeaseOneOfficialTagRule(tagInfos));
        CheckRule(new ParticipantsLimitShouldBeNullOrGreaterThanTwoRule(participantsLimit));

        Id = id;
        _name = name;
        _description = description;
        _participantsLimit = participantsLimit;
        _timeRange = TimeRange.Create(startDate, endDate);
        _imageId = picture;
        var coordinates = Coordinates.From(longitude, latitude);
        _location = Location.From(locationName, locationDescription, coordinates);
        var tagNames = tagInfos.Select(x => x.Name).ToList();
        _tagNames = tagNames;
        _status = MeetingStatus.Upcoming;
        
        _isPublic = isPublic;
        _confidentialInfo = confidentialInfo;

        var owner = Participant.Create(ownerId, MeetingRole.Owner, currentDate);
        _participants.Add(owner);
    }

    public static Expression<Func<Meeting, bool>> ShouldStatusBeChanged(DateTimeOffset currentDate)
         {
        return x => 
            x._status == MeetingStatus.Upcoming 
                && currentDate >= x._timeRange.Start 
                && currentDate <= x._timeRange.End 
            || (x._status == MeetingStatus.Upcoming || x._status == MeetingStatus.Ongoing) 
                && currentDate >= x._timeRange.End;
    }
    
    public static Meeting Create(
        MeetingId id,
        string name,
        string description,
        int? participantsLimit,
        DateTimeOffset startDate,
        DateTimeOffset endDate,
        Guid imageId,
        string locationName,
        string locationDescription,
        double longitude,
        double latitude,
        List<TagInfo> tagInfos,
        UserId userId,
        DateTimeOffset currentDate,
        bool isPublic,
        string? confidentialInfo)
        => new(
            id, 
            name, 
            description, 
            participantsLimit,
            startDate,
            endDate,
            imageId, 
            locationName,
            locationDescription,
            longitude, 
            latitude,
            tagInfos,
            userId,
            currentDate,
            isPublic,
            confidentialInfo);

    public void Edit(
        string newName, 
        string newDescription, 
        int? newParticipantsLimit,
        DateTimeOffset startDate,
        DateTimeOffset endDate,
        Guid newImageId,
        string newLocationName,
        string newLocationDescription,
        double newLongitude,
        double newLatitude,
        List<TagInfo> newTagInfos,
        UserId newUserId,
        DateTimeOffset currentDate,
        bool isPublic,
        string? confidentialInfo)
    {
        CheckRule(new OnlyOwnerOrLeaderCanEditMeetingRule(newUserId, _participants));
        CheckRule(new NumberOfTagsShouldBeBetweenOneAndTenRule(newTagInfos));
        CheckRule(new StartDateOfMeetingCannotBeInThePastRule(startDate, currentDate));
        CheckRule(new MeetingShouldHaveAtLeaseOneOfficialTagRule(newTagInfos));
        CheckRule(new ParticipantsLimitShouldBeNullOrGreaterThanTwoRule(newParticipantsLimit));
        
        _name = newName;
        _description = newDescription;
        _participantsLimit = newParticipantsLimit;
        _timeRange = TimeRange.Create(startDate, endDate);
        _imageId = newImageId;
        var newCoordinates = Coordinates.From(newLongitude, newLatitude);
        _location = Location.From(newLocationName, newLocationDescription, newCoordinates);
        var newTagNames = newTagInfos.Select(x => x.Name).ToList();
        _tagNames = newTagNames;
        _isPublic = isPublic;
        _confidentialInfo = confidentialInfo;
    }
    
    public void AskToJoin(UserId userId)
    {
        CheckRule(new CannotAskToJoinMeetingTwiceRule(userId, _applications));

        var application = MeetingApplication.CreatePending(userId);
        _applications.Add(application);
        
        var ownerId = _participants.Single(x => x.MeetingRole == MeetingRole.Owner).UserId;
        
        AddDomainEvent(new UserAskedToJoinMeetingDomainEvent(
            userId.Value,
            Id.Value,
            ownerId.Value));
    }

    public void Join(UserId userId, DateTimeOffset currentDate)
    {
        CheckRule(new CannotJoinToMeetingTwiceRule(userId, _participants));
        CheckRule(new CannotJoinMeetingWhenParticipantsLimitIsReachedRule(_participants.Count, _participantsLimit));
        
        var participant = Participant.Create(userId, MeetingRole.Member, currentDate);
        _participants.Add(participant);

        var ownerId = _participants.Single(x => x.MeetingRole == MeetingRole.Owner).UserId;
        
        AddDomainEvent(new UserJoinedMeetingDomainEvent(
            userId.Value,
            Id.Value,
            ownerId.Value));
    }

    public void JoinOrAskToJoin(UserId userId, DateTimeOffset currentDate)
    {
        var participantsCount = _participants.Count + 1;
        CheckRule(new NumberOfParticipantsShouldNotExceedParticipantsLimitRule(participantsCount, _participantsLimit));
        if (_isPublic)
        {
            Join(userId, currentDate);
        }
        else
        {
            AskToJoin(userId);
        }
    }
    
    public void AcceptApplication(UserId currentUserId, UserId userToAcceptId, DateTimeOffset currentDate)
    {
        var application = _applications.SingleOrDefault(x => x.UserId == userToAcceptId);
        if (application is not null)
        {
            CheckRule(new OnlyOwnerOrLeaderCanAcceptOrRejectApplicationsRule(currentUserId, _participants));
            CheckRule(new OnlyPendingApplicationCanBeAcceptedRule(application.Status));
            
            application.Accept();
            var participant = Participant.Create(application.UserId, MeetingRole.Member, currentDate);
            _participants.Add(participant);
            
            AddDomainEvent(new ApplicationAcceptedDomainEvent(
                userToAcceptId.Value,
                Id.Value,
                currentUserId.Value));
        }
    }
    
    public void RejectApplication(UserId currentUserId, UserId userToAcceptId)
    {
        var application = _applications.SingleOrDefault(x => x.UserId == userToAcceptId);
        if (application is not null)
        {
            CheckRule(new OnlyOwnerOrLeaderCanAcceptOrRejectApplicationsRule(currentUserId, _participants));
            CheckRule(new OnlyPendingApplicationCanBeRejectedRule(application.Status));
            
            application.Reject();
        }
    }
    
    public void LeaveMeeting(UserId userId)
    {
        CheckRule(new OnlyLeaderOrMemberCanLeaveMeetingRule(userId, _participants));
        
        _participants.RemoveAll(p => p.UserId == userId);
        _applications.RemoveAll(p => p.UserId == userId);
    }

    public void TryCancel(UserId ownerId)
    {
        CheckRule(new OnlyOwnerCanCancelMeetingRule(ownerId, _participants));

        if (_status == MeetingStatus.Upcoming)
        {
            _status = MeetingStatus.Cancelled;   
        }
    }

    private bool ShouldStatusBeChangedToOngoing(DateTimeOffset currentDate)
        => _status == MeetingStatus.Upcoming && _timeRange.IsInRange(currentDate);

    private bool ShouldStatusBeChangedToFinished(DateTimeOffset currentDate)
        => (_status == MeetingStatus.Upcoming || _status == MeetingStatus.Ongoing) 
           && currentDate >= _timeRange.End;

    public void UpdateStatus(DateTimeOffset currentDate)
    {
        if (ShouldStatusBeChangedToOngoing(currentDate))
        {
            _status = MeetingStatus.Ongoing;
        }
        else if (ShouldStatusBeChangedToFinished(currentDate))
        {
            _status = MeetingStatus.Finished;
        }
    }

    public MeetingSnapshot GetSnapshot()
        => new(
            Id.Copy(),
            _name,
            _description,
            _participantsLimit,
            _timeRange.Copy(),
            _imageId,
            _location.Copy(),
            _tagNames.AsReadOnly(),
            _status.Copy(),
            _participants.AsReadOnly(),
            _applications.AsReadOnly());
}