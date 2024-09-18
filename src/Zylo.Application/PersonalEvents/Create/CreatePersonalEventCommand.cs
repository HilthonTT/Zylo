using Zylo.Application.Abstractions.Messaging;

namespace Zylo.Application.PersonalEvents.Create;

public sealed record CreatePersonalEventCommand(
    Guid UserId, 
    string Name, 
    int CategoryId, 
    DateTime DateTime) : ICommand<Guid>;
