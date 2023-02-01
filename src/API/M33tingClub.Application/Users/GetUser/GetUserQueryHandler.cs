using System.Data;
using Dapper;
using M33tingClub.Application.Users.GetUserSelf;
using M33tingClub.Application.Utilities;
using M33tingClub.Domain.Auth;
using M33tingClub.Domain.Users;
using M33tingClub.Domain.Utilities.Exceptions;

namespace M33tingClub.Application.Users.GetUser;

public class GetUserQueryHandler : IQueryHandler<GetUserQuery, UserDetailsDTO>
{
	private IDbConnection _connection;
	private readonly IUserContext _userContext;

	public GetUserQueryHandler(
		ISqlConnectionFactory sqlConnectionFactory, 
		IUserContext userContext)
	{
		_userContext = userContext;
		_connection = sqlConnectionFactory.GetOpenConnection();
	}

	public async Task<UserDetailsDTO> Handle(GetUserQuery query, CancellationToken cancellationToken)
	{
		var searchedUserId = query.Id;
		var currentUserId = _userContext.UserId;

		var sql = "select " +
		          $"\"id\" as {nameof(UserDetailsDTO.Id)}," +
		          $"\"firebase_id\" as {nameof(UserDetailsDTO.FirebaseUid)}," +
		          $"\"name\" as {nameof(UserDetailsDTO.Name)}," +
		          $"\"birthday\" as {nameof(UserDetailsDTO.Birthday)}," +
		          $"\"gender\" as {nameof(UserDetailsDTO.Gender)}, " +
		          $"\"image_id\" as {nameof(UserDetailsDTO.ImageId)}, " +
		          $"\"phone_number\" as {nameof(UserDetailsDTO.PhoneNumber)}, " +
		          $"exists (select 1 from \"app\".\"followership\" " +
					$"where follower_id = @CurrentUserId " +
					$"and following_id = @SearchedUserId) as {nameof(UserDetailsDTO.IsFollowedByYou)}, " +
		          $"exists (select 1 from \"app\".\"followership\" " +
					$"where follower_id = @SearchedUserId " +
					$"and following_id = @CurrentUserId) as {nameof(UserDetailsDTO.IsFollowingYou)} " +
		          "FROM \"app\".\"user\" " +
		          "WHERE \"id\" = @SearchedUserId " +
		          "AND \"is_deleted\" = false ";

		var sqlBuilder = new SqlBuilder();
		var sqlTemplate = sqlBuilder.AddTemplate(sql);
		
		var user = await _connection.QuerySingleOrDefaultAsync<UserDetailsDTO>(
			sqlTemplate.RawSql,
			new
			{
				SearchedUserId = searchedUserId,
				CurrentUserId = currentUserId
			});
        
		if (user is null)
		{
			throw new ObjectNotFoundException($"{nameof(User)} with Id: {query.Id} not found.");
		}

		return user;
	}
}