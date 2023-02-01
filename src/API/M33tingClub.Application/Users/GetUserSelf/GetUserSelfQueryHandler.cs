using System.Data;
using M33tingClub.Application.Utilities;
using M33tingClub.Domain.Auth;

namespace M33tingClub.Application.Users.GetUserSelf;

public class GetUserSelfQueryHandler : IQueryHandler<GetUserSelfQuery, UserDTO>
{
    private IDbConnection _connection;
    private IUserContext _userContext;
    
    public GetUserSelfQueryHandler(ISqlConnectionFactory sqlConnectionFactory, IUserContext userContext)
    {
        _connection = sqlConnectionFactory.GetOpenConnection();
        _userContext = userContext;
    }

    public async Task<UserDTO> Handle(GetUserSelfQuery query, CancellationToken cancellationToken)
    {
        return await UsersSupplier.GetUserOrThrow(_connection, _userContext.FirebaseId);
    }
}