using SharedKernel;
using Zylo.Application.Abstractions.Authentication;
using Zylo.Application.Abstractions.Data;
using Zylo.Application.Abstractions.Messaging;
using Zylo.Domain.Friendships;
using Zylo.Domain.Users;

namespace Zylo.Application.FriendRequests.Reject;

internal sealed class RejectFriendRequestCommandHandler : ICommandHandler<RejectFriendRequestCommand>
{
    private readonly IUserContext _userContext;
    private readonly IFriendRequestRepository _friendRequestRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public RejectFriendRequestCommandHandler(
        IUserContext userContext,
        IFriendRequestRepository friendRequestRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _userContext = userContext;
        _friendRequestRepository = friendRequestRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result> Handle(RejectFriendRequestCommand request, CancellationToken cancellationToken)
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

        Result rejectResult = friendRequest.Reject(_dateTimeProvider.UtcNow);

        if (rejectResult.IsFailure)
        {
            return Result.Failure(rejectResult.Error);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
