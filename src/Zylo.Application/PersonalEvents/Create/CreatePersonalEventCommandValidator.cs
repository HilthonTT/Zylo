using FluentValidation;
using Zylo.Application.Abstractions.Validation;

namespace Zylo.Application.PersonalEvents.Create;

internal sealed class CreatePersonalEventCommandValidator : AbstractValidator<CreatePersonalEventCommand>
{
    public CreatePersonalEventCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithError(ValidationErrors.CreatePersonalEvent.UserIdIsRequired);

        RuleFor(x => x.CategoryId).NotEmpty().WithError(ValidationErrors.CreatePersonalEvent.CategoryIdIsRequired);

        RuleFor(x => x.DateTime).NotEmpty().WithError(ValidationErrors.CreatePersonalEvent.DateAndTimeIsRequired);

        RuleFor(x => x.Name).NotEmpty().WithError(ValidationErrors.CreatePersonalEvent.NameIsRequired);
    }
}
