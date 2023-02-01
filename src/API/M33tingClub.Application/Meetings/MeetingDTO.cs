namespace M33tingClub.Application.Meetings;

public class MeetingDTO
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public int? ParticipantsLimit { get; set; }
    
    public DateTimeOffset StartDate { get; set; }
    
    public DateTimeOffset EndDate { get; set; }

    public Guid ImageId { get; set; }
    
    public string LocationName { get; set; }
    
    public string LocationDescription { get; set; }
    
    public double Longitude { get; set; }
    
    public double Latitude { get; set; }
    
    public double Distance { get; set; }

    public string Status { get; set; }

    public List<string> Tags { get; set; }
    
    public List<ParticipantDTO> Participants { get; set; }
    
    public bool IsPublic { get; set; }
    
    public string? ConfidentialInfo { get; set; }
}