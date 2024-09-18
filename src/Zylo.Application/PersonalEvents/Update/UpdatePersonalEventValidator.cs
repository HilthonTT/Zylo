using FluentValidation;
using Zylo.Application.Abstractions.Validation;

namespace Zylo.Application.PersonalEvents.Update;

internal sealed class UpdatePersonalEventValidator : AbstractValidator<UpdatePersonalEventCommand>
{
    public UpdatePersonalEventValidator()
    {
        RuleFor(x => x.PersonalEventId).NotEmpty().WithError(ValidationErrors.UpdatePersonalEvent.PersonalEventIdIsRequired);

        RuleFor(x => x.Name).NotEmpty().WithError(ValidationErrors.UpdatePersonalEvent.NameIsRequired);

        RuleFor(x => x.DateTime).NotEmpty().WithError(ValidationErrors.UpdatePersonalEvent.DateAndTimeIsRequired);
    }
}
