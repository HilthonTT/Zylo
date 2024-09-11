using SharedKernel;

namespace Zylo.Domain.Invitations.DomainEvents;

public sealed record InvitationRejectedDomainEvent(Guid InvitationId) : IDomainEvent;
