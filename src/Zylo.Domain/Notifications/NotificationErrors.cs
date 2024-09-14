using SharedKernel;

namespace Zylo.Domain.Notifications;

public static class NotificationErrors
{
    public static readonly Error AlreadySent = Error.Conflict(
        "Notification.AlreadySent",
        "The notification has already been sent.");
}
