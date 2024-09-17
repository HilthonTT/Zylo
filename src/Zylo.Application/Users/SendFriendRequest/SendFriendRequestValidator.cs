using FluentValidation;
using Zylo.Application.Abstractions.Validation;

namespace Zylo.Application.Users.SendFriendRequest;

internal sealed class SendFriendRequestValidator : AbstractValidator<SendFriendRequestCommand>
{
    public SendFriendRequestValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithError(ValidationErrors.SendFriendRequest.UserIdIsEmpty);

        RuleFor(x => x.FriendId).NotEmpty().WithError(ValidationErrors.SendFriendRequest.FriendIdIsEmpty);
    }
}
