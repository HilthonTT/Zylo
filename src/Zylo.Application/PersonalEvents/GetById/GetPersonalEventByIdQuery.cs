using Zylo.Application.Abstractions.Messaging;
using Zylo.Contracts.PersonalEvents;

namespace Zylo.Application.PersonalEvents.GetById;

public sealed record GetPersonalEventByIdQuery(Guid PersonalEventId) : IQuery<DetailedPersonalEventResponse>;
