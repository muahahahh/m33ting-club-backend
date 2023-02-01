using System.Data;
using M33tingClub.Application.Utilities;
using Npgsql;

namespace M33tingClub.Infrastructure.DataAccess;

public class NpgsqlConnectionFactory : ISqlConnectionFactory, IDisposable
{
    private readonly string _connectionString;
    private IDbConnection _connection;
    
    public NpgsqlConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public IDbConnection GetOpenConnection()
    {
        if (_connection is null || _connection.State != ConnectionState.Open)
        {
            _connection = new NpgsqlConnection(_connectionString);
            _connection.Open();
        }

        return _connection;
    }

    public void Dispose()
    {
        if (_connection is not null && _connection.State == ConnectionState.Open)
        {
            _connection.Dispose();
        }
    }
}