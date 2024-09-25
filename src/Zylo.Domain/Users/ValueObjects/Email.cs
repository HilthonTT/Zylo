using SharedKernel;
using System.Text.RegularExpressions;

namespace Zylo.Domain.Users.ValueObjects;

public sealed record Email
{
    public const int MaxLength = 512;

    private static readonly Regex EmailRegex = new Regex(
       @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(\.[a-zA-Z]{2,})$",
       RegexOptions.Compiled | RegexOptions.IgnoreCase);



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

        if (!EmailRegex.IsMatch(email))
        {
            return Result.Failure<Email>(EmailErrors.InvalidFormat);
        }

        return new Email(email);
    }
}