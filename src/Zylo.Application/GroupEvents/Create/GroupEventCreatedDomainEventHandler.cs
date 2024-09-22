using SharedKernel;
using Zylo.Application.Abstractions.Data;
using Zylo.Application.Abstractions.Messaging;
using Zylo.Domain.Events;
using Zylo.Domain.Events.DomainEvents;

namespace Zylo.Application.GroupEvents.Create;

internal sealed class GroupEventCreatedDomainEventHandler : IDomainEventHandler<GroupEventCreatedDomainEvent>
{
    private readonly IGroupEventRepository _groupEventRepository;
    private readonly IAttendeeRepository _attendeeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public GroupEventCreatedDomainEventHandler(
        IGroupEventRepository groupEventRepository,
        IAttendeeRepository attendeeRepository,
        IUnitOfWork unitOfWork)
    {
        _groupEventRepository = groupEventRepository;
        _attendeeRepository = attendeeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(GroupEventCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        GroupEvent? groupEvent = await _groupEventRepository.GetByIdAsync(notification.GroupEventId, cancellationToken);
        if (groupEvent is null)
        {
            throw new DomainException(GroupEventErrors.NotFound(notification.GroupEventId));
        }

        Attendee attendee = groupEvent.GetOwner();

        _attendeeRepository.Insert(attendee);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
