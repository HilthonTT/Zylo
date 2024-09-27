using SharedKernel;
using Zylo.Application.Abstractions.Data;
using Zylo.Application.Abstractions.Events;
using Zylo.Application.Abstractions.Messaging;
using Zylo.Domain.Friendships;
using Zylo.Domain.Friendships.DomainEvents;
using Zylo.Domain.Users;

namespace Zylo.Application.FriendRequests.Accepted;

internal sealed class FriendRequestAcceptedDomainEventHandler
    : IDomainEventHandler<FriendRequestAcceptedDomainEvent>
{
    private readonly IEventBus _eventBus;
    private readonly IUserRepository _userRepository;
    private readonly IFriendRequestRepository _friendRequestRepository;
    private readonly IFriendshipRepository _friendshipRepository;
    private readonly IUnitOfWork _unitOfWork;

    public FriendRequestAcceptedDomainEventHandler(
        IEventBus eventBus,
        IUserRepository userRepository,
        IFriendRequestRepository friendRequestRepository,
        IFriendshipRepository friendshipRepository,
        IUnitOfWork unitOfWork)
    {
        _eventBus = eventBus;
        _userRepository = userRepository;
        _friendRequestRepository = friendRequestRepository;
        _friendshipRepository = friendshipRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(FriendRequestAcceptedDomainEvent notification, CancellationToken cancellationToken)
    {
        await _eventBus.PublishAsync(
            new FriendRequestAcceptedIntegrationEvent(notification.FriendRequestId),
            cancellationToken);

        FriendRequest? friendRequest = await _friendRequestRepository.GetByIdAsync(
            notification.FriendRequestId,
            cancellationToken);

        if (friendRequest is null)
        {
            throw new DomainException(FriendRequestErrors.NotFound(notification.FriendRequestId));
        }

        var friendshipService = new FriendshipService(_userRepository, _friendshipRepository);

        await friendshipService.CreateFriendshipAsync(friendRequest, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
