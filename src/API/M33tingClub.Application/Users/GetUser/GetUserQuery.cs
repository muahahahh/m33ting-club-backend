using M33tingClub.Application.Users.GetUserSelf;
using M33tingClub.Application.Utilities;

namespace M33tingClub.Application.Users.GetUser;

public record GetUserQuery(Guid Id) : IQuery<UserDetailsDTO>;