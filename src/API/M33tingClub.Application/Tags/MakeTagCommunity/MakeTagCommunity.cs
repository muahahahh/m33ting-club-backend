using M33tingClub.Application.Utilities;
using MediatR;

namespace M33tingClub.Application.Tags.MakeTagCommunity;

public record MakeTagCommunityCommand(string Name) : ICommand<Unit>;