using SharedKernel;

namespace Zylo.Domain.Events;

public static class PersonalEventErrors
{
    public static Error NotFound(Guid groupEventId) => Error.NotFound(
        "PersonalEvent.NotFound",
        $"The group event with the Id = '{groupEventId}' was not found.");

    public static Error UserNotFound(Guid userId) => Error.NotFound(
        "PersonalEvent.UserNotFound",
        $"The user with the Id = '{userId}' was not found.");

    public static readonly Error DateAndTimeIsInThePast = Error.Problem(
        "PersonalEvent.InThePast",
        "The event date and time cannot be in the past.");

    public static readonly Error AlreadyProcessed = Error.Conflict(
        "PersonalEvent.AlreadyProcessed", 
        "The event has already been processed.");
}
