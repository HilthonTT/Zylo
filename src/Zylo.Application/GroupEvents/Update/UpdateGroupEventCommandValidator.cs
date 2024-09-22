using FluentValidation;
using Zylo.Application.Abstractions.Validation;
using Zylo.Domain.Events.ValueObjects;

namespace Zylo.Application.GroupEvents.Update;

internal sealed class UpdateGroupEventCommandValidator : AbstractValidator<UpdateGroupEventCommand>
{
    public UpdateGroupEventCommandValidator()
    {
        RuleFor(x => x.GroupEventId).NotEmpty().WithError(ValidationErrors.UpdateGroupEvent.GroupEventIdIsRequired);

        RuleFor(x => x.Name)
            .NotEmpty().WithError(ValidationErrors.UpdateGroupEvent.NameIsRequired)
            .MaximumLength(Name.MaxLength).WithError(ValidationErrors.UpdateGroupEvent.NameIsTooLong);

        RuleFor(x => x.DateTime).NotEmpty().WithError(ValidationErrors.UpdateGroupEvent.DateAndTimeIsRequired);
    }
}
