using M33tingClub.Application.Utilities;
using MediatR;

namespace M33tingClub.Application.Tags.MakeTagOfficial;

public record MakeTagOfficialCommand(string Name) : ICommand<Unit>;