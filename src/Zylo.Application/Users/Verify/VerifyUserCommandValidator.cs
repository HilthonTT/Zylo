using FluentValidation;
using Zylo.Application.Abstractions.Validation;

namespace Zylo.Application.Users.Verify;

internal sealed class VerifyUserCommandValidator : AbstractValidator<VerifyUserCommand>
{
    public VerifyUserCommandValidator()
    {
        RuleFor(x => x.Code).NotEmpty().WithError(ValidationErrors.VerifyUser.CodeIsEmpty);
    }
}
