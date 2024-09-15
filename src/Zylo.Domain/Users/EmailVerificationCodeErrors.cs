using SharedKernel;

namespace Zylo.Domain.Users;

public static class EmailVerificationCodeErrors
{
    public static readonly Error Expired = Error.Problem(
        "EmailVerificationCode.Expired",
        "The verification code has expired.");

    public static readonly Error AlreadyVerified = Error.Conflict(
        "EmailVerificationCode.AlreadyVerified",
        "The verification code has already been verified.");
}
