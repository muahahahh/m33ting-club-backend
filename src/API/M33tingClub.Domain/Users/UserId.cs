using M33tingClub.Domain.Utilities;

namespace M33tingClub.Domain.Users;

public class UserId : ValueObject
{
    public Guid Value { get; }

    private UserId()
    {
        // For EF core
    }
    
    private UserId(Guid id)
    {
        Value = id;
    }

    public static UserId CreateNew() => new(Guid.NewGuid());
    
    public static UserId FromGuid(Guid id) => new(id);
}