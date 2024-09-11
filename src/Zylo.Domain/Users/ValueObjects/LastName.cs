using SharedKernel;

namespace Zylo.Domain.Users.ValueObjects;

public sealed record LastName
{
    public const int MaxLength = 128;

    private LastName(string value) => Value = value;

    public string Value { get; }

    public static implicit operator string(LastName firstName) => firstName.Value;

    public static Result<LastName> Create(string? firstName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            return Result.Failure<LastName>(FirstNameErrors.Empty);
        }

        if (firstName.Length > MaxLength)
        {
            return Result.Failure<LastName>(FirstNameErrors.TooLong);
        }

        return new LastName(firstName);
    }
}
