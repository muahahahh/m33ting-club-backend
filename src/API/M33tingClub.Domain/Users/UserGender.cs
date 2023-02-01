using M33tingClub.Domain.Utilities;

namespace M33tingClub.Domain.Users;

public class UserGender : ValueObject
{
    public string Name { get; }

    public static UserGender Male => new(nameof(Male));
    public static UserGender Female => new(nameof(Female));
    public static UserGender Other => new(nameof(Other));
    
    public static UserGender FromName(string name)
        => new(name);
    
    private UserGender(string name)
    {
        Name = name;
    }
    
    public static List<string> GetAll()
    {
        return new List<string>
        {
            Male.Name,
            Female.Name,
            Other.Name,
        };
    }
}