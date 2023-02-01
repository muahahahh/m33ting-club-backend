using System.Data;
using Dapper;
using Newtonsoft.Json;

namespace M33tingClub.Infrastructure.Configuration.Dapper;

public class JsonHandler<T> : SqlMapper.TypeHandler<List<T>>
{
    public override void SetValue(IDbDataParameter parameter, List<T> value)
    {
        parameter.Value = JsonConvert.SerializeObject(value);
    }

    public override List<T> Parse(object value)
    {
        return JsonConvert.DeserializeObject<List<T>>(value as string);
    }
}