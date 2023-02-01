using FluentValidation;
using M33tingClub.Domain.Auth;
using M33tingClub.Domain.Users;

namespace M33tingClub.Application.Users.FinishUserSignUp;

public class FinishUserSignUpCommandValidator : AbstractValidator<SignUpCommand>
{
	private readonly IUserRepository _userRepository;
	
	private readonly IUserContext _userContext;

	public FinishUserSignUpCommandValidator(
		IUserRepository userRepository, 
	    IUserContext userContext)
	{
		_userRepository = userRepository;
		_userContext = userContext;
		
		RuleFor(x => x)
			.Must(_ => CheckUserNotFinishedSignedUp())
			.WithMessage("User already finished sign up.");

		var availableGenders = UserGender.GetAll();
		RuleFor(x => x.Gender)
			.Must(x => availableGenders.Contains(x))
			.WithMessage($"Available values: {string.Join(", ", availableGenders)}.");
	}

	private bool CheckUserNotFinishedSignedUp()
	{
		var user = _userRepository.Get(_userContext.FirebaseId).GetAwaiter().GetResult();

		if (user is null)
		{
			return true;
		}

		return false;
	}
}