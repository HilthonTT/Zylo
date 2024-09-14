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
}
