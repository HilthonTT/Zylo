using SharedKernel;

namespace Zylo.Domain.Users.ValueObjects;

public sealed record Email
{
    public const int MaxLength = 512;

    private Email(string value) => Value = value;

    public string Value { get; }

    public static implicit operator string(Email value) => value.Value;

    public static Result<Email> Create(string? email)
    {
        if (string.IsNullOrEmpty(email))
        {
            return Result.Failure<Email>(EmailErrors.Empty);
        }

        if (email.Length > MaxLength)
        {
            return Result.Failure<Email>(EmailErrors.TooLong);
        }

        if (email.Split('@').Length != 2)
        {
            return Result.Failure<Email>(EmailErrors.InvalidFormat);
        }

        return new Email(email);
    }
}
