using M33tingClub.Application.Utilities;
using MediatR;

namespace M33tingClub.Application.Users.Followers.FollowUser;

public record FollowUserCommand(Guid UserId) : ICommand<Unit>;