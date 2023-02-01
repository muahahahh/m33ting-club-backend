using M33tingClub.Application.Utilities;
using MediatR;

namespace M33tingClub.Application.Users.Followers.UnfollowUser;

public record UnfollowUserCommand(Guid UserId) : ICommand<Unit>;