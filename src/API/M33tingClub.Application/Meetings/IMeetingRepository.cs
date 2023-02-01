using M33tingClub.Domain.Meetings;

namespace M33tingClub.Application.Meetings;

public interface IMeetingRepository
{
    Task Add(Meeting meeting);

    Task<List<Meeting>> GetMultiple(List<MeetingId> ids);

    Task<Meeting> GetOrThrow(MeetingId id);

    Task<List<Meeting>> GetForChangeStatus(DateTimeOffset currentDate);
}