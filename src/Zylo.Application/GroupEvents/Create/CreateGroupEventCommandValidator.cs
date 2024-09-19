using FluentValidation;
using Zylo.Application.Abstractions.Validation;

namespace Zylo.Application.GroupEvents.Create;

internal sealed class CreateGroupEventCommandValidator : AbstractValidator<CreateGroupEventCommand>
{
    public CreateGroupEventCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithError(ValidationErrors.CreateGroupEvent.UserIdIsRequired);

        RuleFor(x => x.Name).NotEmpty().WithError(ValidationErrors.CreateGroupEvent.NameIsRequired);

        RuleFor(x => x.CategoryId).NotEmpty().WithError(ValidationErrors.CreateGroupEvent.CategoryIdIsRequired);

        RuleFor(x => x.DateTime).NotEmpty().WithError(ValidationErrors.CreateGroupEvent.DateAndTimeIsRequired);
    }
}
