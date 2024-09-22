using Zylo.Application.Abstractions.Messaging;

namespace Zylo.Application.GroupEvents.Update;

public sealed record UpdateGroupEventCommand(Guid GroupEventId, string Name, DateTime DateTime) : ICommand;
