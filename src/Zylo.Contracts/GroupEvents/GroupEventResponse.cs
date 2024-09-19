namespace Zylo.Contracts.GroupEvents;

public sealed record GroupEventResponse(
    Guid Id,
    string Name,
    int CategoryId,
    string Category,
    DateTime DateTimeUtc,
    DateTime CreatedOnUtc);
