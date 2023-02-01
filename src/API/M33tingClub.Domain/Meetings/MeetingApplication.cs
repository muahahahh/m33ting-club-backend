using M33tingClub.Domain.Users;
using M33tingClub.Domain.Utilities;

namespace M33tingClub.Domain.Meetings;

public class MeetingApplication : Entity
{
    public UserId UserId { get; }
    
    public MeetingApplicationStatus Status { get; private set; }
    
    public MeetingApplication(UserId userId, MeetingApplicationStatus status)
    {
        UserId = userId;
        Status = status;
    }

    public static MeetingApplication CreatePending(UserId userId)
        => new(userId, MeetingApplicationStatus.Pending);

    public void Accept()
        => Status = MeetingApplicationStatus.Accepted;
    
    public void Reject()
        => Status = MeetingApplicationStatus.Rejected;
}