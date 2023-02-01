using M33tingClub.Domain.Utilities;

namespace M33tingClub.Domain.Meetings;

public class MeetingRole : ValueObject
{
    public string Name { get; }

    public static MeetingRole Owner => new(nameof(Owner));
    
    public static MeetingRole Leader => new(nameof(Leader));
    
    public static MeetingRole Member => new(nameof(Member));

    private MeetingRole(string name)
    {
        Name = name;
    }

    public static MeetingRole FromName(string name)
        => new(name);
    
    public bool IsOwnerOrLeader()
        => this == Owner || this == Leader;
    
    public bool IsLeaderOrMember()
        => this == Leader || this == Member;
}