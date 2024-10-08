﻿using FluentValidation;
using Zylo.Application.Abstractions.Validation;

namespace Zylo.Application.Users.ChangePassword;

internal sealed class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithError(ValidationErrors.ChangePassword.UserIdIsRequired);

        RuleFor(x => x.CurrentPassword).NotEmpty().WithError(ValidationErrors.ChangePassword.CurrentPasswordIsRequired);

        RuleFor(x => x.NewPassword).NotEmpty().WithError(ValidationErrors.ChangePassword.NewPasswordIsRequired);
    }
}
