using Zylo.Application.Abstractions.Messaging;

namespace Zylo.Application.PersonalEvents.Update;

public sealed record UpdatePersonalEventCommand(
    Guid PersonalEventId, 
    string Name, 
    DateTime DateTime) : ICommand;
