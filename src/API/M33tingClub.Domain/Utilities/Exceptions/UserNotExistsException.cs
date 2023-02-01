namespace M33tingClub.Domain.Utilities.Exceptions;

public class UserNotExistsException : Exception
{
    public UserNotExistsException(string firebaseId)
        : base($"User with Firebase Id {firebaseId} not exists.")
    {
    }
}