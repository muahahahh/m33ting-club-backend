namespace M33tingClub.Domain.Auth;

public interface IUserContext
{
    public Guid UserId { get; }

    public Guid? UserIdOptional { get; }
    
    public string FirebaseId { get; }

    public Task InitializeContext();
}