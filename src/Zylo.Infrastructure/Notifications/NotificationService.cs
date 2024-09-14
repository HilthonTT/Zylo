using FluentEmail.Core;
using Zylo.Application.Abstractions.Notifications;
using Zylo.Domain.Users;

namespace Zylo.Infrastructure.Notifications;

internal sealed class NotificationService : INotificationService
{
    private readonly IFluentEmail _fluentEmail;

    public NotificationService(IFluentEmail fluentEmail)
    {
        _fluentEmail = fluentEmail;
    }

    public Task SendAsync(User user, string subject, string body, CancellationToken cancellationToken = default)
    {
        return _fluentEmail
            .To(user.Email)
            .Subject(subject)
            .Body(body)
            .SendAsync(cancellationToken);
    }
}
