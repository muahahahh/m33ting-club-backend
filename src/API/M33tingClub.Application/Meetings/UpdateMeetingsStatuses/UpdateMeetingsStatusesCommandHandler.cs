using M33tingClub.Application.Utilities;
using M33tingClub.Domain.Utilities;
using MediatR;

namespace M33tingClub.Application.Meetings.UpdateMeetingsStatuses;

public class UpdateMeetingsStatusesCommandHandler : ICommandHandler<UpdateMeetingsStatusesCommand, Unit>
{
	private readonly IMeetingRepository _meetingRepository;

	public UpdateMeetingsStatusesCommandHandler(IMeetingRepository meetingRepository)
	{
		_meetingRepository = meetingRepository;
	}

	public async Task<Unit> Handle(UpdateMeetingsStatusesCommand request, CancellationToken cancellationToken)
	{
		var currentDate = Clock.Now;
		var meetings = await _meetingRepository.GetForChangeStatus(currentDate);
		meetings.ForEach(x => x.UpdateStatus(currentDate));
		
		return Unit.Value;
	}
}