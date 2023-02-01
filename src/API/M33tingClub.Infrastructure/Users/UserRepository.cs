using M33tingClub.Application.Users;
using M33tingClub.Domain.Users;
using M33tingClub.Domain.Utilities.Exceptions;
using M33tingClub.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace M33tingClub.Infrastructure.Users;

public class UserRepository : IUserRepository
{
    private readonly M33tingClubDbContext _m33tingClubDbContext;

    public UserRepository(M33tingClubDbContext m33tingClubDbContext)
    {
        _m33tingClubDbContext = m33tingClubDbContext;
    }
    
    public async Task Add(User user)
    {
        await _m33tingClubDbContext.AddAsync(user);
    }

    public async Task<User?> Get(string firebaseId)
    {
        return await _m33tingClubDbContext.Users.SingleOrDefaultAsync(x => !x.IsDeleted && x.FirebaseId == firebaseId);
    }

    public async Task<User> GetOrThrow(UserId id)
    {
        var user = await _m33tingClubDbContext.Users.SingleOrDefaultAsync(x => !x.IsDeleted && x.Id == id);

        if (user is null)
        {
            throw new ObjectNotFoundException($"{nameof(User)} with Id: {id.Value} not found.");
        }

        return user!;
    }
}