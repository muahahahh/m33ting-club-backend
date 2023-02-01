using M33tingClub.Domain.Utilities;

namespace M33tingClub.Domain.MeetingNotifications.Rules;

public class OnlyReceiverCanMarkMeetingNotificationAsSeenRule : IRule
{
    private readonly Guid _receiverId;

    private readonly Guid _markingUserId;
    
    public OnlyReceiverCanMarkMeetingNotificationAsSeenRule(Guid receiverId, Guid markingUserId)
    {
        _receiverId = receiverId;
        _markingUserId = markingUserId;
    }

    public bool IsBroken() => _receiverId != _markingUserId;

    public string Message => "Only receiver can mark meeting notification as seen.";
    public RuleExceptionKind Kind => RuleExceptionKind.BadRequest;
}