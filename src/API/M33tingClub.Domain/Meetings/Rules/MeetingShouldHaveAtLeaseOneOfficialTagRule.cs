using M33tingClub.Domain.Tags;
using M33tingClub.Domain.Utilities;

namespace M33tingClub.Domain.Meetings.Rules;

public class MeetingShouldHaveAtLeaseOneOfficialTagRule : IRule
{
    private readonly List<TagInfo> _tags;

    public MeetingShouldHaveAtLeaseOneOfficialTagRule(List<TagInfo> tags)
    {
        _tags = tags;
    }
    public bool IsBroken()
        => !_tags.Any(x => x.IsOfficial);

    public string Message => "Meeting should have at least one official tag.";
    
    public RuleExceptionKind Kind => RuleExceptionKind.Conflict;
}