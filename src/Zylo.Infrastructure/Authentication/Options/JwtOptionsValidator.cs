using FluentValidation;

namespace Zylo.Infrastructure.Authentication.Options;

internal sealed class JwtOptionsValidator : AbstractValidator<JwtOptions>
{
    public JwtOptionsValidator()
    {
        RuleFor(x => x.Secret).NotEmpty();

        RuleFor(x => x.Issuer).NotEmpty();

        RuleFor(x => x.Audience).NotEmpty();

        RuleFor(x => x.ExpirationInMinutes).NotEmpty().GreaterThan(0);
    }
}
