using FluentValidation;
using Zylo.Application.Abstractions.Validation;

namespace Zylo.Application.Friendships.Remove;

internal sealed class RemoveFriendshipCommandValidator : AbstractValidator<RemoveFriendshipCommand>
{
    public RemoveFriendshipCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithError(ValidationErrors.RemoveFriendship.UserIdIsRequired);

        RuleFor(x => x.FriendId).NotEmpty().WithError(ValidationErrors.RemoveFriendship.FriendIdIsRequired);
    }
}
