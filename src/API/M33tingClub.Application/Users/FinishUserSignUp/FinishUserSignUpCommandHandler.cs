using System.Data;
using FirebaseAdmin.Auth;
using FluentValidation;
using FluentValidation.Results;
using M33tingClub.Application.Utilities;
using M33tingClub.Domain.Auth;
using M33tingClub.Domain.Users;
using MediatR;

namespace M33tingClub.Application.Users.FinishUserSignUp;

public class SignUpCommandHandler : ICommandHandler<SignUpCommand, Unit>
{
    private readonly IUserRepository _userRepository;
    
    private readonly IUserContext _userContext;

    private readonly IUnitOfWork _unitOfWork;

    private readonly IDbConnection _connection;

    public SignUpCommandHandler(
        IUserRepository userRepository, 
        IUserContext userContext, 
        IUnitOfWork unitOfWork,
        ISqlConnectionFactory sqlConnectionFactory)
    {
        _userRepository = userRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
        _connection = sqlConnectionFactory.GetOpenConnection();
    }

    public async Task<Unit> Handle(SignUpCommand command, CancellationToken cancellationToken)
    {
        var phoneNumber = (await FirebaseAuth.DefaultInstance.GetUserAsync(_userContext.FirebaseId, cancellationToken)).PhoneNumber;
        
        if (phoneNumber is null)
        {
            throw new ValidationException(new List<ValidationFailure>
            { 
                new("PhoneNumber",$"Phone number of user with FirebaseId: {_userContext.FirebaseId} do not exists.")
            });
        }
        
        var userWithPhoneNumberExists = await UsersSupplier.GetUserByPhoneNumber(_connection, phoneNumber) is not null;
        if (userWithPhoneNumberExists)
        {
            throw new ValidationException(new List<ValidationFailure>
            {
                new("PhoneNumber", $"Phone number: {phoneNumber} is not unique.")
            });
        }

        var user = User.Create(
            UserId.CreateNew(),
            _userContext.FirebaseId,
            command.Name,
            command.Birthday,
            UserGender.FromName(command.Gender),
            phoneNumber,
            command.ImageId);
        
        await _userRepository.Add(user);
        await _unitOfWork.CommitAsync();
        
        var claims = new Dictionary<string, object>
        {
            { "dbUserId", user.Id.Value.ToString() }
        };

        await FirebaseAuth.DefaultInstance.SetCustomUserClaimsAsync(user.FirebaseId, claims, cancellationToken);

        return await Unit.Task;
    }
}