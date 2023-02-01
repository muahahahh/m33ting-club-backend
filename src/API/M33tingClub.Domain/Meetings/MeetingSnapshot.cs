using M33tingClub.Domain.Tags;

namespace M33tingClub.Domain.Meetings;

public class MeetingSnapshot
{
	public readonly MeetingId Id;
    
	public readonly string Name;

	public readonly string? Description;

	public readonly int? ParticipantsLimit;
    
	public readonly TimeRange TimeRange;
    
	public readonly Guid ImageId;

	public readonly Location Location;

	public readonly IReadOnlyCollection<TagName> TagNames;

	public readonly MeetingStatus Status;

	//TODO: it should be participantSnapshot
	public readonly IReadOnlyCollection<Participant> Participants;

	//TODO: it should be meetingApplicationSnapshot
	public readonly IReadOnlyCollection<MeetingApplication> Applications;

	public MeetingSnapshot(
		MeetingId id, 
		string name, 
		string? description, 
		int? participantsLimit,
		TimeRange timeRange,
		Guid imageId,
		Location location,
		IReadOnlyCollection<TagName> tagNames,
		MeetingStatus status,
		IReadOnlyCollection<Participant> participants,
		IReadOnlyCollection<MeetingApplication> applications)
	{
		Id = id;
		Name = name;
		Description = description;
		ParticipantsLimit = participantsLimit;
		TimeRange = timeRange;
		ImageId = imageId;
		Location = location;
		TagNames = tagNames;
		Status = status;
		Participants = participants;
		Applications = applications;
	}
}