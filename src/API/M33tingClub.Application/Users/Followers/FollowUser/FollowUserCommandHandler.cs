using M33tingClub.Application.Utilities;
using M33tingClub.Domain.Auth;
using M33tingClub.Domain.Users;
using M33tingClub.Domain.Users.Followers;
using M33tingClub.Domain.Utilities;
using MediatR;

namespace M33tingClub.Application.Users.Followers.FollowUser;

public class FollowUserCommandHandler : ICommandHandler<FollowUserCommand, Unit>
{
    private readonly IFollowershipRepository _followershipRepository;
    private readonly IUserContext _userContext;
    
    public FollowUserCommandHandler(IUserContext userContext, IFollowershipRepository followershipRepository)
    {
        _userContext = userContext;
        _followershipRepository = followershipRepository;
    }
    
    public async Task<Unit> Handle(FollowUserCommand command, CancellationToken cancellationToken)
    {
        var followerId = UserId.FromGuid(_userContext.UserId);
        var followingId = UserId.FromGuid(command.UserId);

        var followership = Followership.Create(followerId, followingId, Clock.Now);

        await _followershipRepository.Add(followership);

        return await Unit.Task;
    }
}