using Zylo.Application.Abstractions.Messaging;
using Zylo.Contracts.GroupEvents;

namespace Zylo.Application.GroupEvents.GetById;

public sealed record GetGroupEventByIdQuery(Guid GroupEventId) : IQuery<DetailedGroupEventResponse>;
