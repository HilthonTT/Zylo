namespace Zylo.Contracts.Authentication;

public sealed record ChangePasswordRequest(Guid UserId, string CurrentPassword, string NewPassword);
