using M33tingClub.Application.Utilities;
using M33tingClub.Domain.Auth;
using M33tingClub.Domain.Users;
using MediatR;

namespace M33tingClub.Application.Users.Followers.UnfollowUser;

public class UnfollowUserCommandHandler : ICommandHandler<UnfollowUserCommand, Unit>
{
    private readonly IFollowershipRepository _followershipRepository;
    private readonly IUserContext _userContext;

    public UnfollowUserCommandHandler(IUserContext userContext, IFollowershipRepository followershipRepository)
    {
        _userContext = userContext;
        _followershipRepository = followershipRepository;
    }

    public async Task<Unit> Handle(UnfollowUserCommand command, CancellationToken cancellationToken)
    {
        var followerId = UserId.FromGuid(_userContext.UserId);
        var followingId = UserId.FromGuid(command.UserId);

        var followership = await _followershipRepository.GetOrThrow(followerId, followingId);

        await _followershipRepository.Delete(followership);

        return await Unit.Task;
    }
}