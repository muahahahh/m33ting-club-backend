using M33tingClub.Domain.Utilities;

namespace M33tingClub.Domain.Tags;

public class TagInfo : ValueObject
{
    public TagName Name { get; set; }
    
    public bool IsOfficial { get; set; }

    private TagInfo(TagName name, bool isOfficial)
    {
        Name = name;
        IsOfficial = isOfficial;
    }

    public static TagInfo Create(TagName name, bool isOfficial = false)
        => new(name, isOfficial);
    
}