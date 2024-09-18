using FluentValidation;
using Zylo.Application.Abstractions.Validation;

namespace Zylo.Application.Users.Login;

internal sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithError(ValidationErrors.Login.EmailIsRequired)
            .EmailAddress().WithError(ValidationErrors.Login.BadEmailFormat);

        RuleFor(x => x.Password).NotEmpty().WithError(ValidationErrors.Login.PasswordIsRequired);
    }
}
