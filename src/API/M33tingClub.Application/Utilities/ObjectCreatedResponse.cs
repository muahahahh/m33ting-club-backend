namespace M33tingClub.Application.Utilities;

public class ObjectCreatedResponse
{
	public Guid Id { get; }
	
	public ObjectCreatedResponse(Guid id)
	{
		Id = id;
	}
}