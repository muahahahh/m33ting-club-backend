using System.Runtime.Serialization;

namespace M33tingClub.Application.Meetings;

[DataContract]
public class ParticipantDTO
{
    [DataMember(Name = "user_id")]
    public Guid UserId { get; set; }
    
    [DataMember(Name = "meeting_role")]    
    public string MeetingRole { get; set; }
    
    [DataMember(Name = "name")]    
    public string Name { get; set; }

    [DataMember(Name = "image_id")]    
    public Guid? ImageId { get; set; }
}