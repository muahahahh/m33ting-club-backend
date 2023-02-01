using M33tingClub.Domain.Users;

namespace M33tingClub.Application.Users;

public interface IUserRepository
{
    Task Add(User user);

    Task<User?> Get(string firebaseId);

    Task<User> GetOrThrow(UserId id);
}