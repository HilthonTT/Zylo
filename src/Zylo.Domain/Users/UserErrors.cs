using SharedKernel;

namespace Zylo.Domain.Users;

public static class UserErrors
{
    public static Error NotFound(Guid userId) => Error.NotFound(
        "User.NotFound",
        $"The user with the Id = '{userId}' was not found.");

    public static readonly Error NotFoundByEmail = Error.NotFound(
        "User.NotFoundByEmail",
        "The user with the specified email was not found.");

    public static readonly Error EmailNotUnique = Error.Conflict(
        "User.EmailNotUnique",
        "The provided email is not unique.");

    public static readonly Error InvalidPermissions = Error.Unauthorized(
       "User.InvalidPermissions",
       "You do not have the required permissions to perform this action or access this resource.");

    public static readonly Error CannotChangePassword = Error.Problem(
        "User.CannotChangePassword",
        "The password cannot be changed to the specified password.");
}
