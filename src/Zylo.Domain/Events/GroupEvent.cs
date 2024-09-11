using SharedKernel;
using Zylo.Domain.Events.DomainEvents;
using Zylo.Domain.Events.ValueObjects;
using Zylo.Domain.Invitations;
using Zylo.Domain.Invitations.DomainEvents;
using Zylo.Domain.Users;

namespace Zylo.Domain.Events;

public sealed class GroupEvent : Event
{
    public GroupEvent(Guid userId, Name name, Category category, DateTime dateTimeUtc) 
        : base(userId, name, category, dateTimeUtc, EventType.GroupEvent)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupEvent"/> class.
    /// </summary>
    /// <remarks>
    /// Required by EF Core.
    /// </remarks>
    private GroupEvent()
    {
    }

    public static GroupEvent Create(Guid userId, Name name, Category category, DateTime dateTimeUtc)
    {
        var groupEvent = new GroupEvent(userId, name, category, dateTimeUtc);

        groupEvent.Raise(new GroupEventCreatedDomainEvent(groupEvent.Id));

        return groupEvent;
    }

    public async Task<Result<Invitation>> InviteAsync(
        User user, 
        IInvitationRepository invitationRepository,
        CancellationToken cancellationToken = default)
    {
        if (await invitationRepository.HasAlreadySentAsync(this, user, cancellationToken))
        {
            return Result.Failure<Invitation>(GroupEventErrors.InvitationAlreadySent);
        }

        var invitation = new Invitation(Id, user.Id);

        Raise(new InvitationSentDomainEvent(invitation.Id));

        return invitation;
    }

    public Attendee GetOwner() => new(this);

    public override Result Cancel(DateTime utcNow)
    {
        Result result = base.Cancel(utcNow);

        if (result.IsSuccess)
        {
            Raise(new GroupEventCancelledDomainEvent(Id));
        }

        return result;
    }

    public override bool ChangeName(Name name)
    {
        string previousName = Name;

        bool hasChanged = base.ChangeName(name);

        if (hasChanged)
        {
            Raise(new GroupEventNameChangedDomainEvent(Id, previousName));
        }

        return hasChanged;
    }

    public override bool ChangeDateAndTime(DateTime dateTimeUtc)
    {
        DateTime previousDateAndTime = DateTimeUtc;

        bool hasChanged = base.ChangeDateAndTime(dateTimeUtc);

        if (hasChanged)
        {
            Raise(new GroupEventDateAndTimeChangedDomainEvent(Id, previousDateAndTime));
        }

        return hasChanged;
    }
}
