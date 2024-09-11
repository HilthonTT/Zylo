using SharedKernel;

namespace Zylo.Domain.Users.ValueObjects;

public static class EmailErrors
{
    public static readonly Error Empty = Error.Problem(
        "Email.Empty",
        "Email is empty.");

    public static readonly Error TooLong = Error.Problem(
        "Email.TooLong",
        "Email is too long.");

    public static readonly Error InvalidFormat = Error.Problem(
        "Email.InvalidFormat",
        "Email format is invalid.");
}
