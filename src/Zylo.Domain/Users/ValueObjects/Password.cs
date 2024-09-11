using SharedKernel;

namespace Zylo.Domain.Users.ValueObjects;

public sealed record Password
{
    private const int MinPasswordLength = 6;
    private static readonly Func<char, bool> IsLower = c => c >= 'a' && c <= 'z';
    private static readonly Func<char, bool> IsUpper = c => c >= 'A' && c <= 'Z';
    private static readonly Func<char, bool> IsDigit = c => c >= '0' && c <= '9';
    private static readonly Func<char, bool> IsNonAlphaNumeric = c => !(IsLower(c) || IsUpper(c) || IsDigit(c));

    private Password(string value) => Value = value;

    public string Value { get; }

    public static implicit operator string(Password password) => password.Value;

    public static Result<Password> Create(string? password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return Result.Failure<Password>(PasswordErrors.Empty);
        }

        if (password.Length < MinPasswordLength)
        {
            return Result.Failure<Password>(PasswordErrors.TooShort);
        }

        if (!password.Any(IsDigit))
        {
            return Result.Failure<Password>(PasswordErrors.MissingDigit);
        }

        if (!password.Any(IsUpper))
        {
            return Result.Failure<Password>(PasswordErrors.MissingUppercaseLetter);
        }

        if (!password.Any(IsLower))
        {
            return Result.Failure<Password>(PasswordErrors.MissingLowercaseLetter);
        }

        if (!password.Any(IsNonAlphaNumeric))
        {
            return Result.Failure<Password>(PasswordErrors.MissingNonAlphaNumeric);
        }

        return new Password(password);
    }
}
