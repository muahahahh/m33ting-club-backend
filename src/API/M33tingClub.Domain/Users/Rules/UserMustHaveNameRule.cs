using M33tingClub.Domain.Utilities;

namespace M33tingClub.Domain.Users.Rules;

public class UserMustHaveNameRule : IRule
{
    private readonly string _name;

    public UserMustHaveNameRule(string name)
    {
        _name = name;
    }
    
    public bool IsBroken()
    {
        return string.IsNullOrWhiteSpace(_name);
    }

    public string Message => "User must have a name";

    public RuleExceptionKind Kind => RuleExceptionKind.BadRequest;
}