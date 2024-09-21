using Zylo.Application.Abstractions.Messaging;

namespace Zylo.Application.GroupEvents.Cancel;

public sealed record CancelGroupEventCommand(Guid GroupEventId) : ICommand;
