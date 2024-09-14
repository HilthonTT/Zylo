﻿using FluentValidation;
using Zylo.Application.Abstractions.Validation;

namespace Zylo.Application.Users;

internal sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithError(ValidationErrors.CreateUser.FirstNameIsEmpty);

        RuleFor(x => x.LastName).NotEmpty().WithError(ValidationErrors.CreateUser.LastNameIsEmpty);

        RuleFor(x => x.Email)
            .NotEmpty().WithError(ValidationErrors.CreateUser.EmailIsEmpty)
            .EmailAddress().WithError(ValidationErrors.CreateUser.BadEmailFormat);

        RuleFor(x => x.Password).NotEmpty().WithError(ValidationErrors.CreateUser.PasswordIsEmpty);
    }
}
