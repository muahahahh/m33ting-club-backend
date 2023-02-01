using M33tingClub.Application.Utilities;

namespace M33tingClub.Application.Tags.AddOfficialTag;

public record AddOfficialTagCommand(string Name) : ICommand<NamedObjectCreatedResponse>;