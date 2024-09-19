using Zylo.Application.Abstractions.Messaging;
using Zylo.Contracts.GroupEvents;

namespace Zylo.Application.GroupEvents.GetTenMostRecentAttending;

public sealed record GetTenMostRecentAttendingGroupEventsQuery(
    Guid UserId,
    int NumberOfGroupEventsToTake = 10) : IQuery<List<GroupEventResponse>>;
