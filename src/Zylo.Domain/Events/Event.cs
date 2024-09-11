using SharedKernel;
using Zylo.Domain.Events.ValueObjects;

namespace Zylo.Domain.Events;

public abstract class Event : Entity, IAuditable, ISoftDeletable
{
    protected Event(Guid userId, Name name, Category category, DateTime dateTimeUtc, EventType type)
        : base(Guid.NewGuid())
    {
        Ensure.NotNullOrEmpty(userId, nameof(userId));
        Ensure.NotNullOrEmpty(name, nameof(name));
        Ensure.NotNull(category, nameof(category));
        Ensure.NotNull(dateTimeUtc, nameof(dateTimeUtc));
        Ensure.NotNull(type, nameof(type));

        UserId = userId;
        Name = name;
        Category = category;
        DateTimeUtc = dateTimeUtc;
        Type = type;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Event"/> class.
    /// </summary>
    /// <remarks>
    /// Required by EF Core.
    /// </remarks>
    protected Event()
    {
    }

    public Guid UserId { get; private set; }

    public Name Name { get; private set; }

    public Category Category { get; private set; }

    public DateTime DateTimeUtc { get; private set; }

    public bool Cancelled { get; private set; }

    public EventType Type { get; private set; }

    public DateTime CreatedOnUtc { get; set; }

    public DateTime? ModifiedOnUtc { get; set; }

    public DateTime? DeletedOnUtc { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Result Cancel(DateTime utcNow)
    {
        if (Cancelled)
        {
            return Result.Failure(EventErrors.AlreadyCancelled);
        }

        if (utcNow > DateTimeUtc)
        {
            return Result.Failure(EventErrors.EventHasPassed);
        }

        Cancelled = true;

        return Result.Success();
    }

    public virtual bool ChangeName(Name name)
    {
        if (name == Name)
        {
            return false;
        }

        Name = name;

        return true;
    }

    public virtual bool ChangeDateAndTime(DateTime dateTimeUtc)
    {
        if (dateTimeUtc == DateTimeUtc)
        {
            return false;
        }

        DateTimeUtc = dateTimeUtc;

        return true;
    }
}
