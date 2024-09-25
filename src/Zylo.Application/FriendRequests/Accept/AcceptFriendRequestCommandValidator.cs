using FluentValidation;
using Zylo.Application.Abstractions.Validation;

namespace Zylo.Application.FriendRequests.Accept;

internal sealed class AcceptFriendRequestCommandValidator : AbstractValidator<AcceptFriendRequestCommand>
{
    public AcceptFriendRequestCommandValidator()
    {
        RuleFor(x => x.FriendRequestId)
            .NotEmpty().WithError(ValidationErrors.AcceptFriendshipRequest.FriendshipRequestIdIsRequired);
    }
}
