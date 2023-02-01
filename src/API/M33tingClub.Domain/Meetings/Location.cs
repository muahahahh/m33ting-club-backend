using M33tingClub.Domain.Utilities;

namespace M33tingClub.Domain.Meetings;

public class Location : ValueObject
{
    public string Name { get; }
    
    public string Description { get; }
    
    public Coordinates Coordinates { get; }

    private Location(string name, string description,Coordinates coordinates)
    {
        Name = name;
        Description = description;
        Coordinates = coordinates;
    }

    public static Location From(string name, string description, Coordinates coordinates)
        => new(name, description, coordinates);

    public Location Copy()
        => new(Name, Description, Coordinates.Copy());
}