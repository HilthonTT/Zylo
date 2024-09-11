using Zylo.Domain.Events;
using Zylo.Domain.Users;

namespace Zylo.Domain.Notifications;

public interface INotificationRepository
{
    Task<(Notification Notification, Event Event, User User)[]> GetNotificationsToBeSentIncludingUserAndEvent(
            int batchSize,
            DateTime utcNow,
            int allowedNotificationTimeDiscrepancyInMinutes,
            CancellationToken cancellationToken = default);

    void InsertRange(List<Notification> notifications);

    void Update(Notification notification);

    Task RemoveNotificationsForEventAsync(Guid eventId, DateTime utcNow, CancellationToken cancellationToken = default);
}
