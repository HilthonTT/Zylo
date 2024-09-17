namespace Zylo.Contracts.Users;

public sealed record UserResponse(
    Guid Id, 
    string FirstName, 
    string LastName, 
    DateTime CreatedOnUtc,
    long NumberOfPersonalEvents,
    long NumberOfFriends);
