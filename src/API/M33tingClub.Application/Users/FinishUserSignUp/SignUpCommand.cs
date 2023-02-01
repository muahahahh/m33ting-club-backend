using M33tingClub.Application.Utilities;
using MediatR;

namespace M33tingClub.Application.Users.FinishUserSignUp;

public record SignUpCommand(
	string Name,
	DateTime Birthday, 
	string Gender,
	Guid? ImageId) : ICommand<Unit>;