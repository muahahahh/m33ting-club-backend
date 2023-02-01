using M33tingClub.Application.Users.Followers;
using M33tingClub.Domain.Users;
using M33tingClub.Domain.Users.Followers;
using M33tingClub.Domain.Utilities.Exceptions;
using M33tingClub.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace M33tingClub.Infrastructure.Users.Followers;

public class FollowershipRepository : IFollowershipRepository
{
    private readonly M33tingClubDbContext _m33tingClubDbContext;

    public FollowershipRepository(M33tingClubDbContext m33tingClubDbContext)
    {
        _m33tingClubDbContext = m33tingClubDbContext;
    }
    
    public async Task Add(Followership followership)
    {
        await _m33tingClubDbContext.AddAsync(followership);
    }

    public async Task<Followership> GetOrThrow(UserId followerId, UserId followingId)
    {
        var followerShip = await _m33tingClubDbContext.Followership
            .SingleOrDefaultAsync(x => x.FollowerId == followerId && x.FollowingId == followingId);
        
        if (followerShip is null)
        {
            throw new ObjectNotFoundException($"{nameof(Followership)} with FollowerId: {followerId.Value} and FollowingId: {followingId.Value} not found.");
        }

        return followerShip;
    }

    public async Task Delete(Followership followership)
    {
        _m33tingClubDbContext.Followership.Remove(followership);
        await Task.CompletedTask;
    }
}