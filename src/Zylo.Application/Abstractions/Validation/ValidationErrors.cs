using SharedKernel;

namespace Zylo.Application.Abstractions.Validation;

internal static class ValidationErrors
{
    internal static class CreateUser
    {
        internal static readonly Error FirstNameIsEmpty = Error.Problem(
            "CreateUser.FirstNameIsEmpty", "The first name is empty.");

        internal static readonly Error LastNameIsEmpty = Error.Problem(
            "CreateUser.LastNameIsEmpty", "The last name is empty.");

        internal static readonly Error PasswordIsEmpty = Error.Problem(
            "CreateUser.PasswordIsEmpty", "The password is empty.");

        internal static readonly Error EmailIsEmpty = Error.Problem(
            "CreateUser.EmailIsEmpty", "The email is empty.");

        internal static readonly Error BadEmailFormat = Error.Problem(
            "CreateUser.BadEmailFormat", "The email format is wrong.");
    }

    internal static class VerifyUser
    {
        internal static readonly Error CodeIsEmpty = Error.Problem(
            "VerifyUser.CodeIsEmpty", "Please provide a valid code.");
    }

    internal static class ResendUserEmailVerification
    {
        internal static readonly Error EmailIsEmpty = Error.Problem(
            "ResendUserEmailVerification.EmailIsEmpty", "The email is empty.");

        internal static readonly Error BadEmailFormat = Error.Problem(
            "ResendUserEmailVerification.BadEmailFormat", "The email format is wrong.");
    }

    internal static class SendFriendRequest
    {
        internal static readonly Error UserIdIsEmpty = Error.Problem(
            "SendFriendRequest.UserIdIsEmpty",
            "The user identifier is empty.");

        internal static readonly Error FriendIdIsEmpty = Error.Problem(
            "SendFriendRequest.FriendIdIsEmpty",
            "The friend identifier is empty.");
    }

    internal static class UpdateUser
    {
        internal static readonly Error UserIdIsEmpty = Error.Problem(
            "UpdateUser.UserIdIsEmpty",
            "The user identifier is empty.");

        internal static readonly Error FirstNameIsEmpty = Error.Problem(
            "UpdateUser.FirstNameIsEmpty", 
            "The first name is empty.");

        internal static readonly Error LastNameIsEmpty = Error.Problem(
            "UpdateUser.LastNameIsEmpty", 
            "The last name is empty.");
    }

    internal static class Login
    {
        internal static readonly Error EmailIsEmpty = Error.Problem(
            "Login.EmailIsEmpty", 
            "The email is empty.");

        internal static readonly Error BadEmailFormat = Error.Problem(
            "Login.BadEmailFormat", "The email format is wrong.");

        internal static readonly Error PasswordIsEmpty = Error.Problem(
            "Login.PasswordIsEmpty",
            "The password is required.");
    }

    internal static class ChangePassword
    {
        internal static readonly Error UserIdIsEmpty = Error.Problem(
            "ChangePassword.UserIdIsEmpty", 
            "The user identifier is required.");

        internal static readonly Error CurrentPasswordIsEmpty = Error.Problem(
            "ChangePassword.CurrentPasswordIsEmpty", 
            "The password is required.");

        internal static readonly Error NewPasswordIsEmpty = Error.Problem(
            "ChangePassword.NewPasswordIsEmpty",
            "The password is required.");
    }
}
