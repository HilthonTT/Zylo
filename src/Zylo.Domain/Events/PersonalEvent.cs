using SharedKernel;
using Zylo.Domain.Events.DomainEvents;
using Zylo.Domain.Events.ValueObjects;

namespace Zylo.Domain.Events;

public sealed class PersonalEvent : Event
{
    internal PersonalEvent(Guid userId, Name name, Category category, DateTime dateTimeUtc)
            : base(userId, name, category, dateTimeUtc, EventType.PersonalEvent)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PersonalEvent"/> class.
    /// </summary>
    /// <remarks>
    /// Required by EF Core.
    /// </remarks>
    private PersonalEvent()
    {
    }

    public bool Processed { get; private set; }

    public Result MarkAsProcessed()
    {
        if (Processed)
        {
            return Result.Failure(PersonalEventErrors.AlreadyProcessed);
        }

        Processed = true;

        return Result.Success();
    }

    public override Result Cancel(DateTime utcNow)
    {
        Result result = base.Cancel(utcNow);

        if (result.IsSuccess)
        {
            Raise(new PersonalEventCancelledDomainEvent(Id));
        }

        return result;
    }

    public override bool ChangeDateAndTime(DateTime dateTimeUtc)
    {
        bool hasChanged = base.ChangeDateAndTime(dateTimeUtc);

        if (hasChanged)
        {
            Raise(new PersonalEventDateAndTimeChangedDomainEvent(Id));

            Processed = false;
        }

        return hasChanged;
    }
}
