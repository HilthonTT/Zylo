using Zylo.Application.Abstractions.Messaging;

namespace Zylo.Application.GroupEvents.Create;

public sealed record CreateGroupEventCommand(
    Guid UserId,
    string Name,
    int CategoryId,
    DateTime DateTime) : ICommand<Guid>;
