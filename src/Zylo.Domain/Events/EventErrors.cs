using SharedKernel;

namespace Zylo.Domain.Events;

public static class EventErrors
{
    public static readonly Error AlreadyCancelled = Error.Conflict(
        "Event.AlreadyCancelled", 
        "The event has already been cancelled.");

    public static readonly Error EventHasPassed = Error.Problem(
        "Event.EventHasPassed",
        "The event has already passed and cannot be modified.");
}
