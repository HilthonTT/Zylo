using FluentValidation;
using Zylo.Application.Abstractions.Validation;
using Zylo.Application.Users.ResendEmail;

namespace Zylo.Application.Users.ResendEmailVerification;

internal sealed class ResendUserEmailVerificationCommandValidator
    : AbstractValidator<ResendUserEmailVerificationCommand>
{
    public ResendUserEmailVerificationCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithError(ValidationErrors.ResendUserEmailVerification.EmailIsEmpty)
            .EmailAddress().WithError(ValidationErrors.ResendUserEmailVerification.BadEmailFormat);
    }
}
