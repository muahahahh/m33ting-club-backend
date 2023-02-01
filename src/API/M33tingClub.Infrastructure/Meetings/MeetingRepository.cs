using M33tingClub.Application.Meetings;
using M33tingClub.Domain.Meetings;
using M33tingClub.Domain.Utilities.Exceptions;
using M33tingClub.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace M33tingClub.Infrastructure.Meetings;

public class MeetingRepository : IMeetingRepository
{
	private readonly M33tingClubDbContext _m33TingClubDbContext;

	public MeetingRepository(
		M33tingClubDbContext m33TingClubDbContext)
	{
		_m33TingClubDbContext = m33TingClubDbContext;
	}

	public async Task Add(Meeting meeting)
	{
		await _m33TingClubDbContext.AddAsync(meeting);
	}

	public async Task<List<Meeting>> GetMultiple(List<MeetingId> ids)
		=> await _m33TingClubDbContext.Meetings.Where(x => ids.Contains(x.Id)).ToListAsync();

	public async Task<Meeting> GetOrThrow(MeetingId id)
	{
		var meeting = await _m33TingClubDbContext.Meetings.SingleOrDefaultAsync(x => x.Id == id);

		if (meeting is null)
		{
			throw new ObjectNotFoundException($"{nameof(Meeting)} with Id: {id.Value} not found.");
		}

		return meeting;
	}

	public async Task<List<Meeting>> GetForChangeStatus(DateTimeOffset currentDate)
		=>  await _m33TingClubDbContext.Meetings.Where(Meeting.ShouldStatusBeChanged(currentDate)).ToListAsync();
}