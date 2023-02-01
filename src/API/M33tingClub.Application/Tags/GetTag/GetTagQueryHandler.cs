using System.Data;
using Dapper;
using M33tingClub.Application.Utilities;
using M33tingClub.Domain.Tags;
using M33tingClub.Domain.Utilities.Exceptions;

namespace M33tingClub.Application.Tags.GetTag;

public class GetTagQueryHandler : IQueryHandler<GetTagQuery, TagDTO>
{
	private readonly IDbConnection _connection;

	public GetTagQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
	{
		_connection = sqlConnectionFactory.GetOpenConnection();
	}
	
	public async Task<TagDTO> Handle(GetTagQuery query, CancellationToken cancellationToken)
	{
		var sql = "select " +
		          $"\"name\" as {nameof(TagDTO.Name)} ," +
		          $"\"is_official\" as {nameof(TagDTO.IsOfficial)} " +
		          $"from \"app\".\"v_all_tags\" " +
		          $"where \"name\" = @Name";
		
		var sqlBuilder = new SqlBuilder();
		var sqlTemplate = sqlBuilder.AddTemplate(sql);

		var tag = await _connection.QuerySingleOrDefaultAsync<TagDTO>(sqlTemplate.RawSql, new { Name = query.Name });
		
		if (tag is null)
		{
			throw new ObjectNotFoundException($"{nameof(Tag)} with Name: {query.Name} not found."); 
		}

		return tag;
	}
}