using SharedKernel;
using Zylo.Application.Abstractions.Data;
using Zylo.Application.Abstractions.Messaging;
using Zylo.Domain.Users;

namespace Zylo.Application.Users.Verify;

internal sealed class VerifyUserCommandHandler : ICommandHandler<VerifyUserCommand>
{
    private readonly IEmailVerificationCodeRepository _emailVerificationCodeRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IUnitOfWork _unitOfWork;

    public VerifyUserCommandHandler(
        IEmailVerificationCodeRepository emailVerificationCodeRepository,
        IDateTimeProvider dateTimeProvider,
        IUnitOfWork unitOfWork)
    {
        _emailVerificationCodeRepository = emailVerificationCodeRepository;
        _dateTimeProvider = dateTimeProvider;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(VerifyUserCommand request, CancellationToken cancellationToken)
    {
        EmailVerificationCode? verificationCode = await _emailVerificationCodeRepository.GetByCodeAsync(
            request.Code,
            _dateTimeProvider.UtcNow,
            cancellationToken);

        if (verificationCode is null)
        {
            return Result.Failure(EmailVerificationCodeErrors.Expired);
        }

        if (verificationCode.User.EmailVerified)
        {
            return Result.Failure(EmailVerificationCodeErrors.AlreadyVerified);
        }

        verificationCode.User.VerifyEmail();

        _emailVerificationCodeRepository.Remove(verificationCode);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
