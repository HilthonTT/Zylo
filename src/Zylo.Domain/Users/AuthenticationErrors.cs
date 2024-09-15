using SharedKernel;

namespace Zylo.Domain.Users;

public static class AuthenticationErrors
{
    public static readonly Error InvalidEmailOrPassword = Error.Problem(
        "Authentication.InvalidEmailOrPassword",
        "The specified email or password are incorrect.");

    public static readonly Error EmailUnverified = Error.Problem(
        "Authentication.InvalidEmailOrPassword",
        "The email has not been verified.");
}
