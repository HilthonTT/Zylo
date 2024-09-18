using FluentValidation;
using Zylo.Application.Abstractions.Validation;

namespace Zylo.Application.Users.Create;

internal sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithError(ValidationErrors.CreateUser.FirstNameIsRequired);

        RuleFor(x => x.LastName).NotEmpty().WithError(ValidationErrors.CreateUser.LastNameIsRequired);

        RuleFor(x => x.Email)
            .NotEmpty().WithError(ValidationErrors.CreateUser.EmailIsRequired)
            .EmailAddress().WithError(ValidationErrors.CreateUser.BadEmailFormat);

        RuleFor(x => x.Password).NotEmpty().WithError(ValidationErrors.CreateUser.PasswordIsRequired);
    }
}
