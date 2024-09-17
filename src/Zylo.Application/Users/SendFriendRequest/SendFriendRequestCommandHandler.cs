using SharedKernel;
using Zylo.Application.Abstractions.Authentication;
using Zylo.Application.Abstractions.Data;
using Zylo.Application.Abstractions.Messaging;
using Zylo.Domain.Friendships;
using Zylo.Domain.Users;

namespace Zylo.Application.Users.SendFriendRequest;

internal sealed class SendFriendRequestCommandHandler : ICommandHandler<SendFriendRequestCommand>
{
    private readonly IUserContext _userContext;
    private readonly IUserRepository _userRepository;
    private readonly IFriendshipRepository _friendshipRepository;
    private readonly IFriendRequestRepository _friendRequestRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SendFriendRequestCommandHandler(
        IUserContext userContext,
        IUserRepository userRepository,
        IFriendshipRepository friendshipRepository,
        IFriendRequestRepository friendRequestRepository,
        IUnitOfWork unitOfWork)
    {
        _userContext = userContext;
        _userRepository = userRepository;
        _friendshipRepository = friendshipRepository;
        _friendRequestRepository = friendRequestRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(SendFriendRequestCommand request, CancellationToken cancellationToken)
    {
        if (request.UserId != _userContext.UserId)
        {
            return Result.Failure(UserErrors.InvalidPermissions);
        }

        User? user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
        {
            return Result.Failure(UserErrors.NotFound(request.UserId));
        }

        User? friend = await _userRepository.GetByIdAsync(request.FriendId, cancellationToken);
        if (friend is null)
        {
            return Result.Failure(UserErrors.NotFound(request.FriendId));
        }

        Result<FriendRequest> friendRequestResult = await user.SendFriendshipRequestAsync(
            friend.Id,
            _friendshipRepository,
            _friendRequestRepository,
            cancellationToken);

        if (friendRequestResult.IsFailure)
        {
            return Result.Failure(friendRequestResult.Error);
        }

        _friendRequestRepository.Insert(friendRequestResult.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
