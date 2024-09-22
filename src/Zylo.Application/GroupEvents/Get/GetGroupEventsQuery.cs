using Zylo.Application.Abstractions.Messaging;
using Zylo.Contracts.Common;
using Zylo.Contracts.GroupEvents;

namespace Zylo.Application.GroupEvents.Get;

public sealed record GetGroupEventsQuery(
    Guid UserId,
    string? Name,
    int? CategoryId,
    DateTime? StartDate,
    DateTime? EndDate,
    int Page,
    int PageSize) : IQuery<PagedList<GroupEventResponse>>;
