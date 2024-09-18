using Zylo.Application.Abstractions.Messaging;

namespace Zylo.Application.PersonalEvents.Cancel;

public sealed record CancelPersonalEventCommand(Guid PersonalEventId) : ICommand;
