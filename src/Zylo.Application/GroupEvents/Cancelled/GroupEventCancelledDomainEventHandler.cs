using SharedKernel;
using Zylo.Application.Abstractions.Events;
using Zylo.Application.Abstractions.Messaging;
using Zylo.Domain.Events;
using Zylo.Domain.Events.DomainEvents;
using Zylo.Domain.Invitations;
using Zylo.Domain.Notifications;

namespace Zylo.Application.GroupEvents.Cancelled;

internal sealed class GroupEventCancelledDomainEventHandler : IDomainEventHandler<GroupEventCancelledDomainEvent>
{
    private readonly IEventBus _eventBus;
    private readonly IInvitationRepository _invitationRepository;
    private readonly IAttendeeRepository _attendeeRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public GroupEventCancelledDomainEventHandler(
        IEventBus eventBus,
        IInvitationRepository invitationRepository,
        IAttendeeRepository attendeeRepository,
        INotificationRepository notificationRepository,
        IDateTimeProvider dateTimeProvider)
    {
        _eventBus = eventBus;
        _invitationRepository = invitationRepository;
        _attendeeRepository = attendeeRepository;
        _notificationRepository = notificationRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task Handle(GroupEventCancelledDomainEvent notification, CancellationToken cancellationToken)
    {
        await _eventBus.PublishAsync(
            new GroupEventCancelledIntegrationEvent(notification.GroupEventId),
            cancellationToken);

        DateTime now = _dateTimeProvider.UtcNow;

        Task removeAttendeesTask = _attendeeRepository.RemoveAttendeesForGroupEventAsync(
            notification.GroupEventId,
            now,
            cancellationToken);

        Task removeInvitationsTask = _invitationRepository.RemoveInvitationsForGroupEventAsync(
            notification.GroupEventId,
            now,
            cancellationToken);

        Task removeNotificationsTask = _notificationRepository.RemoveNotificationsForEventAsync(
            notification.GroupEventId,
            now,
            cancellationToken);

        await Task.WhenAll(removeAttendeesTask, removeInvitationsTask, removeNotificationsTask);
    }
}
