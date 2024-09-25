using SharedKernel;
using Zylo.Application.Abstractions.Messaging;
using Zylo.Domain.Friendships;
using Zylo.Domain.Friendships.DomainEvents;
using Zylo.Domain.Invitations;

namespace Zylo.Application.Friendships.Remove;

internal sealed class FriendshipRemovedDomainEventHandler : IDomainEventHandler<FriendshipRemovedDomainEvent>
{
    private readonly IFriendshipRepository _friendshipRepository;
    private readonly IInvitationRepository _invitationRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public FriendshipRemovedDomainEventHandler(
        IFriendshipRepository friendshipRepository,
        IInvitationRepository invitationRepository,
        IDateTimeProvider dateTimeProvider)
    {
        _friendshipRepository = friendshipRepository;
        _invitationRepository = invitationRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task Handle(FriendshipRemovedDomainEvent notification, CancellationToken cancellationToken)
    {
        Friendship? friendship = await _friendshipRepository.GetByIdAsync(notification.FriendshipId, cancellationToken);
        if (friendship is null)
        {
            throw new DomainException(FriendshipErrors.NotFriends);
        }

        await _invitationRepository.RemovePendingInvitationsForFriendshipAsync(
            friendship, 
            _dateTimeProvider.UtcNow, 
            cancellationToken);
    }
}
