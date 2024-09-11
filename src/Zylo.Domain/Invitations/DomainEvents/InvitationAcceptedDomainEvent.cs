using SharedKernel;

namespace Zylo.Domain.Invitations.DomainEvents;

public sealed record InvitationAcceptedDomainEvent(Guid InvitationId) : IDomainEvent;
