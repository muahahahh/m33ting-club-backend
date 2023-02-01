using M33tingClub.Domain.Utilities;

namespace M33tingClub.Domain.Meetings;

public class MeetingId : ValueObject
{
    public Guid Value { get; }

    private MeetingId()
    {
        
    }
    
    private MeetingId(Guid id)
    {
        Value = id;
    }

    public static MeetingId CreateNew() => new(Guid.NewGuid());

    public static MeetingId FromGuid(Guid id) => new(id);

    public MeetingId Copy()
        => new(Value);
}