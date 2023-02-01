using System.Data;
using Dapper;
using M33tingClub.Application.Meetings.UpdateMeetingsStatuses;
using M33tingClub.Application.Utilities;
using M33tingClub.Domain.Utilities;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace M33tingClub.Infrastructure.Scheduling;

[DisallowConcurrentExecution]
public class UpdateMeetingsStatusesJob : IJob
{
	private readonly IServiceProvider _provider;

	public UpdateMeetingsStatusesJob(IServiceProvider provider)
	{
		_provider = provider;
	}
	
	public async Task Execute(IJobExecutionContext context)
	{
		using (var scope = _provider.CreateScope())
		{
			var connectionFactory = scope.ServiceProvider.GetService<ISqlConnectionFactory>();
			var connection = connectionFactory!.GetOpenConnection();
			var mediator = scope.ServiceProvider.GetService<IMediator>();
			
			var jobId = await InsertIntoJobHistory(connection);
			
			await mediator!.Send(new UpdateMeetingsStatusesCommand());
			
			await UpdateJobHistory(jobId, connection);
		}
	}
	
	private async Task<Guid> InsertIntoJobHistory(IDbConnection connection)
	{
		var sql =
			"insert into \"app\".\"job_history\" (\"id\", \"name\", \"start_date\") " +
			"values (@Id, @Name, @StartDate)";

		var jobId = Guid.NewGuid();
		var name = "UpdateMeetingStatuses";
		var startDate = Clock.Now;

		await connection.ExecuteAsync(sql, new
		{
			Id = jobId, 
			Name = name, 
			StartDate = startDate
		});
		return jobId;
	}

	private async Task UpdateJobHistory(Guid jobId, IDbConnection connection)
	{
		var sql =
			"update \"app\".\"job_history\" SET \"end_date\" = @EndDate " +
			"where \"id\" = @JobId";
		
		var endDate = Clock.Now;

		await connection.ExecuteAsync(sql, new
		{
			JobId = jobId, 
			EndDate = endDate
		});
	}
}