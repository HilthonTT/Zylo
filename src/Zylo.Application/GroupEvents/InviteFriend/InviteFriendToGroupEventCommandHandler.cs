using SharedKernel;
using Zylo.Application.Abstractions.Authentication;
using Zylo.Application.Abstractions.Data;
using Zylo.Application.Abstractions.Messaging;
using Zylo.Domain.Events;
using Zylo.Domain.Friendships;
using Zylo.Domain.Invitations;
using Zylo.Domain.Users;

namespace Zylo.Application.GroupEvents.InviteFriend;

internal sealed class InviteFriendToGroupEventCommandHandler : ICommandHandler<InviteFriendToGroupEventCommand>
{
    private readonly IUserContext _userContext;
    private readonly IGroupEventRepository _groupEventRepository;
    private readonly IInvitationRepository _invitationRepository;
    private readonly IFriendshipRepository _friendshipRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public InviteFriendToGroupEventCommandHandler(
        IUserContext userContext,
        IGroupEventRepository groupEventRepository,
        IInvitationRepository invitationRepository,
        IFriendshipRepository friendshipRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _userContext = userContext;
        _groupEventRepository = groupEventRepository;
        _invitationRepository = invitationRepository;
        _friendshipRepository = friendshipRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(InviteFriendToGroupEventCommand request, CancellationToken cancellationToken)
    {
        GroupEvent? groupEvent = await _groupEventRepository.GetByIdAsync(request.GroupEventId);

        if (groupEvent is null)
        {
            return Result.Failure(GroupEventErrors.NotFound(request.GroupEventId));
        }

        if (groupEvent.UserId != _userContext.UserId)
        {
            return Result.Failure(UserErrors.InvalidPermissions);
        }

        User? user = await _userRepository.GetByIdAsync(groupEvent.UserId);

        if (user is null)
        {
            return Result.Failure(GroupEventErrors.UserNotFound(request.FriendId));
        }

        User? friend = await _userRepository.GetByIdAsync(request.FriendId);

        if (friend is null)
        {
            return Result.Failure(GroupEventErrors.FriendNotFound(request.FriendId));
        }

        if (!await _friendshipRepository.CheckIfFriendsAsync(user.Id, friend.Id, cancellationToken))
        {
            return Result.Failure(GroupEventErrors.NotFriends);
        }

        Result<Invitation> invitationResult = await groupEvent.InviteAsync(friend, _invitationRepository, cancellationToken);

        if (invitationResult.IsFailure)
        {
            return Result.Failure(invitationResult.Error);
        }

        Invitation invitation = invitationResult.Value;

        _invitationRepository.Insert(invitation);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
