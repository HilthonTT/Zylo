using Microsoft.EntityFrameworkCore;
using Zylo.Domain.Events;
using Zylo.Domain.Notifications;
using Zylo.Domain.Users;

namespace Zylo.Persistence.Repositories;

internal sealed class NotificationRepository : INotificationRepository
{
    private readonly ZyloDbContext _context;

    public NotificationRepository(ZyloDbContext context)
    {
        _context = context;
    }

    public async Task<(Notification Notification, Event Event, User User)[]> GetNotificationsToBeSentIncludingUserAndEventAsync(
        int batchSize, 
        DateTime utcNow, 
        int allowedNotificationTimeDiscrepancyInMinutes, 
        CancellationToken cancellationToken = default)
    {
        DateTime startTime = utcNow.AddMinutes(-allowedNotificationTimeDiscrepancyInMinutes);
        DateTime endTime = utcNow.AddMinutes(allowedNotificationTimeDiscrepancyInMinutes);

        var notificationsWithUsersAndEvents = await _context.Notifications
            .Where(notification => !notification.Sent &&
                                   notification.DateTimeUtc >= startTime &&
                                   notification.DateTimeUtc <= endTime)
            .Join(_context.Events,
                notification => notification.EventId,
                @event => @event.Id,
                (notification, @event) => new { notification, @event })
            .Join(_context.Users,
                notificationEvent => notificationEvent.notification.UserId,
                user => user.Id,
                (notificationEvent, user) => new
                {
                    Notification = notificationEvent.notification,
                    Event = notificationEvent.@event,
                    User = user
                })
            .OrderBy(x => x.Notification.DateTimeUtc)
            .Take(batchSize)
            .ToArrayAsync(cancellationToken);

        return notificationsWithUsersAndEvents.Select(x => (x.Notification, x.Event, x.User)).ToArray();
    }

    public void InsertRange(List<Notification> notifications)
    {
        _context.Notifications.AddRange(notifications);
    }

    public Task RemoveNotificationsForEventAsync(Guid eventId, DateTime utcNow, CancellationToken cancellationToken = default)
    {
        return _context.Notifications
            .Where(e => e.EventId == eventId && !e.IsDeleted)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(e => e.EventId, eventId)
                .SetProperty(e => e.IsDeleted, true)
                .SetProperty(e => e.DeletedOnUtc, utcNow),
                cancellationToken);
    }

    public void Update(Notification notification)
    {
        _context.Notifications.Update(notification);
    }
}
