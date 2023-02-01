using M33tingClub.Domain.Utilities;

namespace M33tingClub.Domain.Meetings;

public class Coordinates : ValueObject
{
    public double Longitude { get; }
    public double Latitude { get; }
    
    private Coordinates(double longitude, double latitude)
    {
        //TODO Validation
        Longitude = longitude;
        Latitude = latitude;
    }

    public static Coordinates From(double longitude, double latitude)
        => new(longitude, latitude);

    public Coordinates Copy()
        => new(Longitude, Latitude);
}