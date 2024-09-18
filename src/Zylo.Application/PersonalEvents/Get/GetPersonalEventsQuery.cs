using Zylo.Application.Abstractions.Messaging;
using Zylo.Contracts.Common;
using Zylo.Contracts.PersonalEvents;

namespace Zylo.Application.PersonalEvents.Get;

public sealed record GetPersonalEventsQuery(
    Guid UserId,
    string? Name,
    int? CategoryId,
    DateTime? StartDate,
    DateTime? EndDate,
    int Page,
    int PageSize) : IQuery<PagedList<PersonalEventResponse>>;
