namespace M33tingClub.Application.Utilities;

public class NamedObjectCreatedResponse
{
    public string Name { get; }
	
    public NamedObjectCreatedResponse(string name)
    {
        Name = name;
    }
}