namespace M33tingClub.Domain.Utilities;

public interface IRule
{
    bool IsBroken();

    string Message { get; }
    
    RuleExceptionKind Kind { get; }
}