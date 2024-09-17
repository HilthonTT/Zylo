using SharedKernel;
using Zylo.Application.Abstractions.Authentication;
using Zylo.Application.Abstractions.Data;
using Zylo.Application.Abstractions.Messaging;
using Zylo.Domain.Users;
using Zylo.Domain.Users.ValueObjects;

namespace Zylo.Application.Users.ChangePassword;

internal sealed class ChangePasswordCommandHandler : ICommandHandler<ChangePasswordCommand>
{
    private readonly IUserContext _userContext;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public ChangePasswordCommandHandler(
        IUserContext userContext,
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork)
    {
        _userContext = userContext;
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        if (request.UserId != _userContext.UserId)
        {
            return Result.Failure(UserErrors.InvalidPermissions);
        }

        Result<Password> currentPasswordResult = Password.Create(request.CurrentPassword);
        Result<Password> newPasswordResult = Password.Create(request.NewPassword);

        Result firstFailureOrSuccess = Result.FirstFailureOrSuccess(currentPasswordResult, newPasswordResult);

        if (firstFailureOrSuccess.IsFailure)
        {
            return Result.Failure(firstFailureOrSuccess.Error);
        }

        User? user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
        {
            return Result.Failure(UserErrors.NotFound(request.UserId));
        }

        bool verified = _passwordHasher.Verify(request.CurrentPassword, user.PasswordHash);
        if (!verified)
        {
            return Result.Failure(AuthenticationErrors.InvalidEmailOrPassword);
        }

        string passwordHash = _passwordHasher.Hash(request.NewPassword);

        Result result = user.ChangePassword(passwordHash);

        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
