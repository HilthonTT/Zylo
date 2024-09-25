using SharedKernel;
using Zylo.Application.Abstractions.Authentication;
using Zylo.Application.Abstractions.Data;
using Zylo.Application.Abstractions.Messaging;
using Zylo.Domain.Friendships;
using Zylo.Domain.Users;

namespace Zylo.Application.FriendRequests.Accept;

internal sealed class AcceptFriendRequestHandler : ICommandHandler<AcceptFriendRequestCommand>
{
    private readonly IUserContext _userContext;
    private readonly IFriendRequestRepository _friendRequestRepository;
    private readonly IFriendshipRepository _friendshipRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public AcceptFriendRequestHandler(
        IUserContext userContext,
        IFriendRequestRepository friendRequestRepository,
        IFriendshipRepository friendshipRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _userContext = userContext;
        _friendRequestRepository = friendRequestRepository;
        _friendshipRepository = friendshipRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result> Handle(AcceptFriendRequestCommand request, CancellationToken cancellationToken)
    {
        FriendRequest? friendRequest = await _friendRequestRepository.GetByIdAsync(request.FriendRequestId, cancellationToken);

        if (friendRequest is null)
        {
            return Result.Failure(FriendRequestErrors.NotFound(request.FriendRequestId));
        }

        if (friendRequest.UserId != _userContext.UserId)
        {
            return Result.Failure(UserErrors.InvalidPermissions);
        }

        User? user = await _userRepository.GetByIdAsync(friendRequest.UserId, cancellationToken);
        if (user is null)
        {
            return Result.Failure(FriendRequestErrors.UserNotFound(friendRequest.UserId));
        }

        User? friend = await _userRepository.GetByIdAsync(friendRequest.FriendId, cancellationToken);
        if (friend is null)
        {
            return Result.Failure(FriendRequestErrors.FriendNotFound(friendRequest.FriendId));
        }

        Result result = friendRequest.Accept(_dateTimeProvider.UtcNow);

        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
