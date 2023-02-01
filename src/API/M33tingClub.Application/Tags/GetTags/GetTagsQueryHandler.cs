using System.Data;
using Dapper;
using M33tingClub.Application.Utilities;

namespace M33tingClub.Application.Tags.GetTags;

public class GetTagsQueryHandler : IQueryHandler<GetTagsQuery, List<TagDTO>>
{
    private readonly IDbConnection _connection;

    public GetTagsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _connection = sqlConnectionFactory.GetOpenConnection();
    }
    
    public async Task<List<TagDTO>> Handle(GetTagsQuery query, CancellationToken cancellationToken)
    {
        var sql = "select " +
                      $"\"name\" as {nameof(TagDTO.Name)} ," +
                      $"\"is_official\" as {nameof(TagDTO.IsOfficial)} " +
                      $"from \"app\".\"v_all_tags\"" +
                      $"/**where**/ ";

        var isOfficial = query.IsOfficial;
        var sqlBuilder = new SqlBuilder();
        var sqlTemplate = sqlBuilder.AddTemplate(sql);

        switch (isOfficial)
        {
            case true:
                sqlBuilder.Where("is_official = true");
                break;
            case false:
                sqlBuilder.Where("is_official = false");
                break;
        }
        
        return (await _connection.QueryAsync<TagDTO>(sqlTemplate.RawSql, new { IsOfficial =  isOfficial})).ToList();

    }
}