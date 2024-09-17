using SharedKernel;
using Zylo.Application.Abstractions.Events;
using Zylo.Application.Abstractions.Notifications;
using Zylo.Domain.Users;

namespace Zylo.Application.Users.ChangePassword;

internal sealed class PasswordChangedIntegrationEventHandler : IIntegrationEventHandler<UserPasswordChangedIntegrationEvent>
{
    private readonly IUserRepository _userRepository;
    private readonly INotificationService _notificationService;

    public PasswordChangedIntegrationEventHandler(
        IUserRepository userRepository,
        INotificationService notificationService)
    {
        _userRepository = userRepository;
        _notificationService = notificationService;
    }

    public async Task Handle(UserPasswordChangedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        User? user = await _userRepository.GetByIdAsync(notification.Id, cancellationToken);

        if (user is null)
        {
            throw new DomainException(UserErrors.NotFound(notification.Id));
        }

        await _notificationService.SendAsync(
            user,
            "Password changed 🔐",
            $"Hello {user.FullName}," +
            Environment.NewLine +
            Environment.NewLine +
            "Your password was successfully changed.",
            cancellationToken);
    }
}
