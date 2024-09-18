namespace Zylo.Contracts.PersonalEvents;

public sealed record PersonalEventResponse(
    Guid Id,
    string Name,
    int CategoryId,
    string Category,
    DateTime DateTimeUtc,
    DateTime CreatedOnUtc);
