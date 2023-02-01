using M33tingClub.Domain.Utilities;

namespace M33tingClub.Domain.Tags;

public class Tag : Entity 
{
    public TagName Name { get; }

    private bool _isOfficial;

    private Tag(TagName name, bool isOfficial)
    {
        Name = name;
        _isOfficial = isOfficial;
    }

    public Tag()
    {
        // For EF core
    }

    public static Tag CreateCommunity(TagName name)
        => new(name, false);

    public static Tag CreateOfficial(TagName name)
        => new(name, true);

    public void MakeOfficial()
    {
        _isOfficial = true;
    }

    public void MakeCommunity()
        => _isOfficial = false;

    public TagInfo GetInfo()
        => TagInfo.Create(Name, _isOfficial);
}
