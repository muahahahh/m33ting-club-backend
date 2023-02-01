using M33tingClub.Domain.Users;
using M33tingClub.Domain.Utilities;

namespace M33tingClub.Domain.Meetings;

public class Participant : Entity
{
    public UserId UserId { get; }
    
    public MeetingRole MeetingRole { get; private set; }
    
    public DateTimeOffset JoinedDate { get; private set; }

    private Participant(
        UserId userId, 
        MeetingRole meetingRole,
        DateTimeOffset joinedDate)
    {
        UserId = userId;
        MeetingRole = meetingRole;
        JoinedDate = joinedDate;
    }

    public static Participant Create(UserId userId, MeetingRole role, DateTimeOffset joinedOn)
        => new(userId, role, joinedOn);

    public void ChangeRole(MeetingRole newRole)
        => MeetingRole = newRole;
}