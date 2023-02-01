namespace M33tingClub.Domain.Utilities.Exceptions;

public class RuleValidationException : Exception
{
    public IRule BrokenRule { get; }

    public string Details { get; }

    public RuleValidationException(IRule brokenRule)
        : base(brokenRule.Message)
    {
        BrokenRule = brokenRule;
        Details = brokenRule.Message;
    }

    public override string ToString()
        => $"{BrokenRule.GetType().FullName}: {BrokenRule.Message}";
}