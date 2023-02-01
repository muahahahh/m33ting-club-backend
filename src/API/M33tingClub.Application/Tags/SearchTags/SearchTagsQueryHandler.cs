using System.Data;
using Dapper;
using M33tingClub.Application.Utilities;

namespace M33tingClub.Application.Tags.SearchTags;

public class SearchTagsQueryHandler : IQueryHandler<SearchTagsQuery, PagingInfo<TagDTO>>
{
    private readonly IDbConnection _connection;

    public SearchTagsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _connection = sqlConnectionFactory.GetOpenConnection();
    }
    
    public async Task<PagingInfo<TagDTO>> Handle(SearchTagsQuery query, CancellationToken cancellationToken)
    {
        var tags = await GetTags(query.Phrase, query.Limit, query.Offset);

        var totalNumberOfTags = await GetTotalNumberOfTags(query.Phrase);

        return new PagingInfo<TagDTO>(tags, totalNumberOfTags);
    }

    private async Task<List<TagDTO>> GetTags(string? phrase, int limit, int offset)
    {
        var sql = "select " +
                  $"\"name\" as {nameof(TagDTO.Name)} ," +
                  $"\"is_official\" as {nameof(TagDTO.IsOfficial)} " +
                  "from \"app\".\"v_all_tags\"" +
                  "/**where**/ " +
                  "/**orderby**/";;
        
        var sqlBuilder = new SqlBuilder();
        var sqlTemplate = sqlBuilder.AddTemplate(sql);

        if (!string.IsNullOrWhiteSpace(phrase))
        {
            sqlBuilder.Where("LOWER(\"name\") LIKE @Phrase");
        }

        sqlBuilder.OrderBy("\"is_official\" desc");
        
        sqlTemplate = sqlBuilder.AddTemplate($"{sqlTemplate.RawSql} LIMIT @Limit OFFSET @Offset");
        
        return (await _connection.QueryAsync<TagDTO>(
            sqlTemplate.RawSql,
            new
            {
                Phrase = $"%{phrase?.ToLower()}%",
                Limit = limit,
                Offset = offset
            })).ToList();
    }
    
    private async Task<int> GetTotalNumberOfTags(string? phrase)
    {
        var sql = "select count(*)" +
                  "from \"app\".\"v_all_tags\"" +
                  "/**where**/ " +
                  "/**orderby**/";;
        
        var sqlBuilder = new SqlBuilder();
        var sqlTemplate = sqlBuilder.AddTemplate(sql);

        if (!string.IsNullOrWhiteSpace(phrase))
        {
            sqlBuilder.Where("LOWER(\"name\") LIKE @Phrase");
        }

        return await _connection.ExecuteScalarAsync<int>(
            sqlTemplate.RawSql,
            new
            {
                Phrase = $"%{phrase?.ToLower()}%",
            });
    }
}