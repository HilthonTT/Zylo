using SharedKernel;

namespace Zylo.Domain.Events;

public static class AttendeeErrors
{
    public static Error NotFound => Error.NotFound(
        "Attendee.NotFound",
        "The attendee with the specified identifier was not found.");

    public static readonly Error AlreadyProcessed = Error.Conflict(
        "Attendee.AlreadyProcessed",
        "The attendee has already been processed.");
}
