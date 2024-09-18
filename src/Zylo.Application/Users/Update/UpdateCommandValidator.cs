using FluentValidation;
using Zylo.Application.Abstractions.Validation;

namespace Zylo.Application.Users.Update;

internal sealed class UpdateCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithError(ValidationErrors.UpdateUser.UserIdIsRequired);

        RuleFor(x => x.FirstName).NotEmpty().WithError(ValidationErrors.UpdateUser.FirstNameIsRequired);

        RuleFor(x => x.LastName).NotEmpty().WithError(ValidationErrors.UpdateUser.LastNameIsRequired);
    }
}
