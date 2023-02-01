using M33tingClub.Application.Utilities;

namespace M33tingClub.Application.Tags.AddCommunityTag;

public record AddCommunityTagCommand(string Name) : ICommand<NamedObjectCreatedResponse>;