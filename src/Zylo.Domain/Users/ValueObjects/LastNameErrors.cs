using SharedKernel;

namespace Zylo.Domain.Users.ValueObjects;

public static class LastNameErrors
{
    public static readonly Error Empty = Error.Problem(
       "LastName.Empty",
       "The last name is empty.");

    public static readonly Error TooLong = Error.Problem(
        "LastName.TooLong",
        "The last name is too long");
}
