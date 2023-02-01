using System.Data;
using FirebaseAdmin.Auth;
using M33tingClub.Application.Meetings;
using M33tingClub.Application.Utilities;
using M33tingClub.Domain.Auth;
using M33tingClub.Domain.Meetings;
using M33tingClub.Domain.Users;
using MediatR;

namespace M33tingClub.Application.Users.DeleteUser;

public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand, Unit>
{
    private IUserRepository _userRepository;
    
    private IUserContext _userContext;

    private IUnitOfWork _unitOfWork;

    private IDbConnection _connection;
    
    private IMeetingRepository _meetingRepository;

    public DeleteUserCommandHandler(
        IUserRepository userRepository, 
        IUserContext userContext, 
        IUnitOfWork unitOfWork, 
        ISqlConnectionFactory connectionFactory, 
        IMeetingRepository meetingRepository)
    {
        _userRepository = userRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
        _connection = connectionFactory.GetOpenConnection();
        _meetingRepository = meetingRepository;
    }
    
    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;

        var user = await _userRepository.GetOrThrow(UserId.FromGuid(userId));
        
        user.MarkAsDeleted();

        var hostMeetingIds = await MeetingsSupplier.GetMeetingsIdsWhichUserHosts(_connection, userId);

        var meetings = await _meetingRepository.GetMultiple(
            hostMeetingIds.Select(MeetingId.FromGuid).ToList());

        foreach (var meeting in meetings)
        {
            meeting.TryCancel(UserId.FromGuid(userId));
        }
        
        await _unitOfWork.CommitAsync();

        await FirebaseAuth.DefaultInstance.DeleteUserAsync(_userContext.FirebaseId, cancellationToken);
        
        return Unit.Value;
    }
}