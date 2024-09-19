using SharedKernel;

namespace Zylo.Application.Abstractions.Validation;

internal static class ValidationErrors
{
    internal static class CreateUser
    {
        internal static readonly Error FirstNameIsRequired = Error.Problem(
            "CreateUser.FirstNameIsRequired", "The first name is required.");

        internal static readonly Error LastNameIsRequired = Error.Problem(
            "CreateUser.LastNameIsRequired", "The last name is required.");

        internal static readonly Error PasswordIsRequired = Error.Problem(
            "CreateUser.PasswordIsRequired", "The password is emrequiredpty.");

        internal static readonly Error EmailIsRequired = Error.Problem(
            "CreateUser.EmailIsRequired", "The email is required.");

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
        internal static readonly Error EmailIsRequired = Error.Problem(
            "ResendUserEmailVerification.EmailIsRequired", "The email is required.");

        internal static readonly Error BadEmailFormat = Error.Problem(
            "ResendUserEmailVerification.BadEmailFormat", "The email format is wrong.");
    }

    internal static class SendFriendRequest
    {
        internal static readonly Error UserIdIsRequired = Error.Problem(
            "SendFriendRequest.UserIdIsRequired",
            "The user identifier is required.");

        internal static readonly Error FriendIdIsRequired = Error.Problem(
            "SendFriendRequest.FriendIdIsRequired", "The friend identifier is required.");
    }

    internal static class UpdateUser
    {
        internal static readonly Error UserIdIsRequired = Error.Problem(
            "UpdateUser.UserIdIsRequired", "The user identifier is required.");

        internal static readonly Error FirstNameIsRequired = Error.Problem(
            "UpdateUser.FirstNameIsRequired", "The first name is required.");

        internal static readonly Error LastNameIsRequired = Error.Problem(
            "UpdateUser.LastNameIsRequired", "The last name is required.");
    }

    internal static class Login
    {
        internal static readonly Error EmailIsRequired = Error.Problem(
            "Login.EmailIsRequired", "The email is required.");

        internal static readonly Error BadEmailFormat = Error.Problem(
            "Login.BadEmailFormat", "The email format is wrong.");

        internal static readonly Error PasswordIsRequired = Error.Problem(
            "Login.PasswordIsRequired", "The password is required.");
    }

    internal static class ChangePassword
    {
        internal static readonly Error UserIdIsRequired = Error.Problem(
            "ChangePassword.UserIdIsRequired", 
            "The user identifier is required.");

        internal static readonly Error CurrentPasswordIsRequired = Error.Problem(
            "ChangePassword.CurrentPasswordIsRequired", 
            "The password is required.");

        internal static readonly Error NewPasswordIsRequired = Error.Problem(
            "ChangePassword.NewPasswordIsRequired",
            "The password is required.");
    }

    internal static class CreatePersonalEvent
    {
        internal static readonly Error UserIdIsRequired = Error.Problem(
            "CreatePersonalEvent.UserIdIsRequired",
            "The user identifier is required.");

        internal static readonly Error NameIsRequired = Error.Problem(
            "CreatePersonalEvent.NameIsRequired",
            "The event name is required.");

        internal static readonly Error CategoryIdIsRequired = Error.Problem(
            "CreatePersonalEvent.CategoryIdIsRequired",
            "The category identifier is required.");

        internal static readonly Error DateAndTimeIsRequired = Error.Problem(
            "CreatePersonalEvent.DateAndTimeIsRequired",
            "The date and time of the event is required.");
    }

    internal static class CancelPersonalEvent
    {
        internal static readonly Error PersonalEventIdIsRequired = Error.Problem(
            "CancelPersonalEvent.PersonalEventIdIsRequired",
            "The personal event identifier is required.");
    }

    internal static class UpdatePersonalEvent
    {
        internal static readonly Error PersonalEventIdIsRequired = Error.Problem(
            "UpdatePersonalEvent.PersonalEventIdIsRequired",
            "The group event identifier is required.");

        internal static readonly Error NameIsRequired = Error.Problem(
            "UpdatePersonalEvent.NameIsRequired", 
            "The event name is required.");

        internal static readonly Error DateAndTimeIsRequired = Error.Problem(
            "UpdatePersonalEvent.DateAndTimeIsRequired",
            "The date and time of the event is required.");
    }

    internal static class CreateGroupEvent
    {
        internal static readonly Error UserIdIsRequired = Error.Problem(
            "CreateGroupEvent.UserIdIsRequired",
            "The user identifier is required.");

        internal static readonly Error NameIsRequired = Error.Problem(
            "CreateGroupEvent.NameIsRequired", 
            "The event name is required.");

        internal static readonly Error CategoryIdIsRequired = Error.Problem(
            "CreateGroupEvent.CategoryIdIsRequired",
            "The category identifier is required.");

        internal static readonly Error DateAndTimeIsRequired = Error.Problem(
            "CreateGroupEvent.DateAndTimeIsRequired",
            "The date and time of the event is required.");
    }
}
