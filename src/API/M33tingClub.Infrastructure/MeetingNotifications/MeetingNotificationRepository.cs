using M33tingClub.Application.MeetingNotifications;
using M33tingClub.Domain.MeetingNotifications;
using M33tingClub.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace M33tingClub.Infrastructure.MeetingNotifications;

public class MeetingNotificationRepository : IMeetingNotificationRepository
{
    private readonly M33tingClubDbContext _m33TingClubDbContext;

    public MeetingNotificationRepository(
        M33tingClubDbContext m33TingClubDbContext)
    {
        _m33TingClubDbContext = m33TingClubDbContext;
    }
    
    public async Task Add(MeetingNotification meetingNotification)
    {
        await _m33TingClubDbContext.MeetingNotifications.AddAsync(meetingNotification);
    }

    public async Task<List<MeetingNotification>> GetMultiple(List<Guid> ids)
        => await _m33TingClubDbContext.MeetingNotifications.Where(x => ids.Contains(x.Id)).ToListAsync();
}