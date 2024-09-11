using SharedKernel;

namespace Zylo.Domain.Users.ValueObjects;

public sealed record FirstName
{
    public const int MaxLength = 128;

    private FirstName(string value) => Value = value;

    public string Value { get; }

    public static implicit operator string(FirstName firstName) => firstName.Value;

    public static Result<FirstName> Create(string? firstName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            return Result.Failure<FirstName>(FirstNameErrors.Empty);
        }

        if (firstName.Length > MaxLength)
        {
            return Result.Failure<FirstName>(FirstNameErrors.TooLong);
        }

        return new FirstName(firstName);
    }
}
