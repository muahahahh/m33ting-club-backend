using M33tingClub.Domain.Tags;
using M33tingClub.Domain.Utilities;

namespace M33tingClub.Domain.Meetings.Rules;

public class NumberOfTagsShouldBeBetweenOneAndTenRule : IRule
{
    private readonly List<TagInfo> _tags;

    public NumberOfTagsShouldBeBetweenOneAndTenRule(List<TagInfo> tags)
    {
        _tags = tags;
    }

    public bool IsBroken()
    {
        return _tags.Count is < 1 or > 10;
    }

    public string Message => "Number of tags should be between 1 and 10";
    
    public RuleExceptionKind Kind => RuleExceptionKind.BadRequest;
}