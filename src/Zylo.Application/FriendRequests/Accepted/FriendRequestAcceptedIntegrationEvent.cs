using Zylo.Application.Abstractions.Events;

namespace Zylo.Application.FriendRequests.Accepted;

public sealed record FriendRequestAcceptedIntegrationEvent(Guid Id) : IntegrationEvent(Id);
