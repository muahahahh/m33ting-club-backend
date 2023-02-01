using System;
using System.Collections.Generic;
using System.Linq;
using M33tingClub.Domain.Meetings;
using M33tingClub.Domain.Tags;
using M33tingClub.Domain.Users;
using M33tingClub.Domain.Utilities;

namespace M33tingClub.UnitTests.Meetings.Meetings;
// TODO figure out a better solution to use meetingbuilder in integration tests
public class MeetingBuilder
{
    private string _name;

    private string? _description;

    private int? _participantsLimit;

    private DateTimeOffset _startDate;

    private DateTimeOffset _endDate;

    private Guid _imageId;

    private string? _locationName;

    private string? _locationDescription;
    
    private double _longitude;

    private double _latitude;

    private List<string> _tagNames;

    private UserId _ownerId;

    private bool _isPublic;

    private string? _confidentialInfo;

    public MeetingBuilder()
    {
        _name = "Default Name";
        _description = "Default Description";
        _participantsLimit = 10;
        _startDate = Clock.Now.AddDays(1);
        _endDate = Clock.Now.AddDays(2);
        _imageId = Guid.Parse("1b1b7e93-4505-4d58-8b44-d81bfe4412aa");
        _locationName = "Default Location";
        _locationDescription = "Default Location Description";
        _longitude = 0;
        _latitude = 0;
        _tagNames = new List<string> {"Tag_1", "Tag_2"};
        _ownerId = UserId.CreateNew();
        _isPublic = true;
        _confidentialInfo = "Some confidential info";
    }

    public MeetingBuilder WithOwner(UserId ownerId)
    {
        _ownerId = ownerId;
        return this;
    }

    internal MeetingBuilder AddTag(string tagName)
    {
        _tagNames.Add(tagName);
        return this;
    }

    internal MeetingBuilder Tags(List<string> tagNames)
    {
        _tagNames.AddRange(tagNames);
        return this;
    }

    internal MeetingBuilder WithStartDate(DateTimeOffset startDate)
    {
        _startDate = startDate;
        return this;
    }
    
    internal MeetingBuilder WithEndDate(DateTimeOffset endDate)
    {
        _endDate = endDate;
        return this;
    }

    internal MeetingBuilder WithParticipantsLimit(int participantsLimit)
    {
        _participantsLimit = participantsLimit;
        return this;
    }
    
    internal MeetingBuilder WithoutParticipantsLimit()
    {
        _participantsLimit = null;
        return this;
    }

    internal MeetingBuilder Public()
    {
        _isPublic = true;
        return this;
    }
    
    internal MeetingBuilder Private()
    {
        _isPublic = false;
        return this;
    }

    internal MeetingBuilder WithConfidentialInfo(string confidentialInfo)
    {
        _confidentialInfo = confidentialInfo;
        return this;
    }
    
    public Meeting Build()
    {
        //TODO: change
        var tags = _tagNames.Select(x => TagInfo.Create(TagName.Create(x), true)).ToList();

        return Meeting.Create(
            MeetingId.CreateNew(), 
            _name,
            _description,
            _participantsLimit,
            _startDate,
            _endDate,
            _imageId,
            _locationName,
            _locationDescription,
            _longitude,
            _latitude,
            tags,
            _ownerId,
            Clock.Now,
            _isPublic,
            _confidentialInfo);
    }
}