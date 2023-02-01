using System.Threading.Tasks;
using Dapper;
using Npgsql;

namespace M33tingClub.IntegrationTests.Utilities;

internal static class DatabaseCleaner
{
    public static async Task Clean(string connectionString, bool cleanUsers = false)
    {
        string sql = 
            "DELETE FROM \"app\".\"meeting_notification\";" + 
            "DELETE FROM \"app\".\"followership\";" +
            "DELETE FROM \"app\".\"participant\";" +
            "DELETE FROM \"app\".\"meeting_tag\";" +
            "DELETE FROM \"app\".\"application\";" +
            "DELETE FROM \"app\".\"tag\";" +
            "DELETE FROM \"app\".\"meeting\";" + 
            (cleanUsers ? "DELETE FROM \"app\".\"user\";" : string.Empty);

        using (var connection = new NpgsqlConnection(connectionString))
        {
            await connection.ExecuteScalarAsync(sql);
        }
    }
}