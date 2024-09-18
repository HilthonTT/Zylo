namespace Zylo.Contracts.PersonalEvents;

public sealed record DetailedPersonalEventResponse(
    Guid Id,
    string Name,
    int CategoryId,
    string Category,
    string CreatedBy,
    DateTime DateTimeUtc,
    DateTime CreatedOnUtc);
