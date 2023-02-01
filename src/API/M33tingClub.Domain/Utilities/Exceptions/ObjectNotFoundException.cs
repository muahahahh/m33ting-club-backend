namespace M33tingClub.Domain.Utilities.Exceptions;

public class ObjectNotFoundException : Exception
{
	public ObjectNotFoundException(string message)
		: base(message)
	{
	}
}