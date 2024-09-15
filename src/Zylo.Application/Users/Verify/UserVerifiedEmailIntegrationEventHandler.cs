using SharedKernel;
using Zylo.Application.Abstractions.Events;
using Zylo.Application.Abstractions.Notifications;
using Zylo.Domain.Users;

namespace Zylo.Application.Users.Verify;

internal sealed class UserVerifiedEmailIntegrationEventHandler : IIntegrationEventHandler<UserVerifiedEmailIntegrationEvent>
{
    private readonly IUserRepository _userRepository;
    private readonly INotificationService _notificationService;

    public UserVerifiedEmailIntegrationEventHandler(
        IUserRepository userRepository,
        INotificationService notificationService)
    {
        _userRepository = userRepository;
        _notificationService = notificationService;
    }

    public async Task Handle(UserVerifiedEmailIntegrationEvent notification, CancellationToken cancellationToken)
    {
        User? user = await _userRepository.GetByIdAsync(notification.Id, cancellationToken);

        if (user is null)
        {
            throw new DomainException(UserErrors.NotFound(notification.Id));
        }

        await _notificationService.SendAsync(
            user,
            "Welcome to Zylo! 🎉",
            $"You verified your account, you may now use Zylo to its full extent!",
            cancellationToken);
    }
}
