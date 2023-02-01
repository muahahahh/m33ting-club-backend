using M33tingClub.Domain.Utilities.Exceptions;

namespace M33tingClub.Domain.Utilities;

//TODO: use for validation in application layer
public static class RuleChecker
{
    public static void CheckRule(IRule rule)
    {
        if (rule.IsBroken())
            throw new RuleValidationException(rule);
    }
}