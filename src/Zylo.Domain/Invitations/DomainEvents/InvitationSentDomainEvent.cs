using SharedKernel;

namespace Zylo.Domain.Invitations.DomainEvents;

public sealed record InvitationSentDomainEvent(Guid InvitationId) : IDomainEvent;
