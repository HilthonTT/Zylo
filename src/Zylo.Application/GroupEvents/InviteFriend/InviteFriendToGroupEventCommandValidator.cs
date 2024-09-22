using FluentValidation;
using Zylo.Application.Abstractions.Validation;

namespace Zylo.Application.GroupEvents.InviteFriend;

internal sealed class InviteFriendToGroupEventCommandValidator : AbstractValidator<InviteFriendToGroupEventCommand>
{
    public InviteFriendToGroupEventCommandValidator()
    {

        RuleFor(x => x.GroupEventId).NotEmpty().WithError(ValidationErrors.InviteFriendToGroupEvent.GroupEventIdIsRequired);

        RuleFor(x => x.FriendId).NotEmpty().WithError(ValidationErrors.InviteFriendToGroupEvent.FriendIdIsRequired);
    }
}
