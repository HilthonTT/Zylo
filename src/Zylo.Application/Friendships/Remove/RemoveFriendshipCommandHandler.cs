using SharedKernel;
using Zylo.Application.Abstractions.Authentication;
using Zylo.Application.Abstractions.Data;
using Zylo.Application.Abstractions.Messaging;
using Zylo.Domain.Friendships;
using Zylo.Domain.Users;

namespace Zylo.Application.Friendships.Remove;

internal sealed class RemoveFriendshipCommandHandler : ICommandHandler<RemoveFriendshipCommand>
{
    private readonly IUserContext _userContext;
    private readonly IUserRepository _userRepository;
    private readonly IFriendshipRepository _friendshipRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveFriendshipCommandHandler(
        IUserContext userContext,
        IUserRepository userRepository,
        IFriendshipRepository friendshipRepository,
        IUnitOfWork unitOfWork)
    {
        _userContext = userContext;
        _userRepository = userRepository;
        _friendshipRepository = friendshipRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(RemoveFriendshipCommand request, CancellationToken cancellationToken)
    {
        if (request.UserId != _userContext.UserId)
        {
            return Result.Failure(UserErrors.InvalidPermissions);
        }

        var friendshipService = new FriendshipService(_userRepository, _friendshipRepository);

        Result result = await friendshipService.RemoveFriendshipAsync(request.UserId, request.FriendId, cancellationToken);

        if (result.IsFailure)
        {
            return result;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
