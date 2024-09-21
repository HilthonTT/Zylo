using FluentValidation;
using Zylo.Application.Abstractions.Validation;

namespace Zylo.Application.GroupEvents.Cancel;

internal sealed class CancelGroupEventCommandValidator : AbstractValidator<CancelGroupEventCommand>
{
    public CancelGroupEventCommandValidator()
    {
        RuleFor(x => x.GroupEventId).NotEmpty().WithError(ValidationErrors.CancelGroupEvent.GroupEventIdIsRequired);
    }
}
