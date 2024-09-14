using Zylo.Domain.Users;

namespace Zylo.Application.Abstractions.Notifications;

public interface INotificationService
{
    Task SendAsync(User user, string subject, string body, CancellationToken cancellationToken = default);
}
