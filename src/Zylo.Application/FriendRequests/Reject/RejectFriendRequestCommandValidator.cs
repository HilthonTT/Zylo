using FluentValidation;
using Zylo.Application.Abstractions.Validation;

namespace Zylo.Application.FriendRequests.Reject;

internal sealed class RejectFriendRequestCommandValidator : AbstractValidator<RejectFriendRequestCommand>
{
    public RejectFriendRequestCommandValidator()
    {
        RuleFor(x => x.FriendRequestId)
            .NotEmpty()
            .WithError(ValidationErrors.RejectFriendshipRequest.FriendshipRequestIdIsRequired);
    }
}
