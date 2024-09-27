using Zylo.Application.Abstractions.Events;

namespace Zylo.Application.FriendRequests.Sent;

public sealed record FriendRequestSentIntegrationEvent(Guid Id) : IntegrationEvent(Id); 
