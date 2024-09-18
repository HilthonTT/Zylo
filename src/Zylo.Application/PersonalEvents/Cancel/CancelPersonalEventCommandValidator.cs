using FluentValidation;
using Zylo.Application.Abstractions.Validation;

namespace Zylo.Application.PersonalEvents.Cancel;

internal sealed class CancelPersonalEventCommandValidator : AbstractValidator<CancelPersonalEventCommand>
{
    public CancelPersonalEventCommandValidator()
    {
        RuleFor(x => x.PersonalEventId).NotEmpty().WithError(ValidationErrors.CancelPersonalEvent.PersonalEventIdIsRequired);
    }
}
