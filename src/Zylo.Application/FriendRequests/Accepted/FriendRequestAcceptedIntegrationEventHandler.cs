using SharedKernel;
using Zylo.Application.Abstractions.Events;
using Zylo.Application.Abstractions.Notifications;
using Zylo.Domain.Friendships;
using Zylo.Domain.Users;

namespace Zylo.Application.FriendRequests.Accepted;

internal sealed class FriendRequestAcceptedIntegrationEventHandler 
    : IIntegrationEventHandler<FriendRequestAcceptedIntegrationEvent>
{
    private readonly IFriendRequestRepository _friendRequestRepository;
    private readonly IUserRepository _userRepository;
    private readonly INotificationService _notificationService;

    public FriendRequestAcceptedIntegrationEventHandler(
        IFriendRequestRepository friendRequestRepository,
        IUserRepository userRepository,
        INotificationService notificationService)
    {
        _friendRequestRepository = friendRequestRepository;
        _userRepository = userRepository;
        _notificationService = notificationService;
    }

    public async Task Handle(FriendRequestAcceptedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        FriendRequest? friendRequest = await _friendRequestRepository.GetByIdAsync(notification.Id, cancellationToken);

        if (friendRequest is null)
        {
            throw new DomainException(FriendRequestErrors.NotFound(notification.Id));
        }

        User? user = await _userRepository.GetByIdAsync(friendRequest.UserId, cancellationToken);

        if (user is null)
        {
            throw new DomainException(FriendRequestErrors.UserNotFound(friendRequest.UserId));
        }

        User? friend = await _userRepository.GetByIdAsync(friendRequest.FriendId, cancellationToken);

        if (friend is null)
        {
            throw new DomainException(FriendRequestErrors.FriendNotFound(friendRequest.FriendId));
        }

        await _notificationService.SendAsync(
            user,
            $"Friend request accepted 😁",
            $"Hello {user.FullName}," +
            Environment.NewLine +
            Environment.NewLine +
            $"The user {friend.FullName} has accepted your friendship request.",
            cancellationToken);
    }
}
