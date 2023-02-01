using System.Data;

namespace M33tingClub.Application.Utilities;

public interface ISqlConnectionFactory
{
    IDbConnection GetOpenConnection();
}