using M33tingClub.Domain.Utilities;

namespace M33tingClub.Domain.Tags;

public class TagName : ValueObject
{
    public string Value { get; }

    public TagName()
    {
        // For EF core
    }
    
    private TagName(string name)
    {
        Value = name;
    }

    public static TagName Create(string name)
        => new (name);
}