using SharedKernel;

namespace Zylo.Domain.Users.ValueObjects;

public static class FirstNameErrors
{
    public static readonly Error Empty = Error.Problem(
        "FirstName.Empty",
        "The first name is empty.");

    public static readonly Error TooLong = Error.Problem(
        "FirstName.TooLong",
        "The first name is too long");
}
