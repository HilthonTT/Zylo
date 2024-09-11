using SharedKernel;

namespace Zylo.Domain.Events.ValueObjects;

public sealed record Name
{
    public const int MaxLength = 100;

    private Name(string value) => Value = value;

    public string Value { get; }

    public static implicit operator string(Name name) => name.Value;

    public static Result<Name> Create(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<Name>(NameErrors.Empty);
        }

        if (name.Length > MaxLength)
        {
            return Result.Failure<Name>(NameErrors.TooLong);
        }

        return new Name(name);
    }
}
