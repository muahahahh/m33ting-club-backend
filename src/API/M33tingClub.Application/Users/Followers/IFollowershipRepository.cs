using M33tingClub.Domain.Users;
using M33tingClub.Domain.Users.Followers;

namespace M33tingClub.Application.Users.Followers;

public interface IFollowershipRepository
{
    Task Add(Followership followership);

    Task<Followership> GetOrThrow(UserId followerId, UserId followingId);
    
    Task Delete(Followership followership);
}