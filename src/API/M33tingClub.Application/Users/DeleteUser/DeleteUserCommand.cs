using M33tingClub.Application.Utilities;
using MediatR;

namespace M33tingClub.Application.Users.DeleteUser;

public record DeleteUserCommand : ICommand<Unit>;