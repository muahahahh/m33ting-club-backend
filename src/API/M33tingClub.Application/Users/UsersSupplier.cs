using System.Data;
using Dapper;
using M33tingClub.Application.Users.GetUserSelf;
using M33tingClub.Domain.Users;
using M33tingClub.Domain.Utilities.Exceptions;

namespace M33tingClub.Application.Users;

internal static class UsersSupplier
{
     private static (SqlBuilder sqlBuilder, SqlBuilder.Template sqlTemplate) CreateBaseQuery()
     {
         var sql = "select " +
                   $"\"id\" as {nameof(UserDTO.Id)}," +
                   $"\"firebase_id\" as {nameof(UserDTO.FirebaseUid)}," +
                   $"\"name\" as {nameof(UserDTO.Name)}," +
                   $"\"birthday\" as {nameof(UserDTO.Birthday)}," +
                   $"\"gender\" as {nameof(UserDTO.Gender)}, " +
                   $"\"image_id\" as {nameof(UserDTO.ImageId)}, " +
                   $"\"phone_number\" as {nameof(UserDTO.PhoneNumber)} " +
                   "FROM \"app\".\"v_user\" " +
                   "/**where**/";

        var sqlBuilder = new SqlBuilder();
        var sqlTemplate = sqlBuilder.AddTemplate(sql);

        return (sqlBuilder, sqlTemplate);
    }

     internal static async Task<UserDTO> GetUserOrThrow(
        IDbConnection connection, 
        string firebaseId)
    {
        var (sqlBuilder, sqlTemplate) = CreateBaseQuery();
        
        sqlBuilder.Where("\"is_deleted\" = false");
        sqlBuilder.Where("\"firebase_id\" = @FirebaseId");

        var user = await connection.QuerySingleOrDefaultAsync<UserDTO>(
            sqlTemplate.RawSql,
            new { FirebaseId = firebaseId});

        if (user is null)
        {
            throw new ObjectNotFoundException($"{nameof(User)} with Firebase Id: {firebaseId} not found.");
        }
        
        return user;
    }
    
    internal static async Task<UserDTO?> GetUserByPhoneNumber(
        IDbConnection connection, 
        string phoneNumber)
    {
        var (sqlBuilder, sqlTemplate) = CreateBaseQuery();

        sqlBuilder.Where("\"is_deleted\" = false");
        sqlBuilder.Where("\"phone_number\" = @PhoneNumber");

        return await connection.QuerySingleOrDefaultAsync<UserDTO>(
            sqlTemplate.RawSql,
            new { PhoneNumber = phoneNumber});
    }
}