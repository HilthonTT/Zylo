namespace Zylo.Contracts.Authentication;

public sealed record RegisterRequest(
    string Email, 
    string FirstName, 
    string LastName, 
    string Password);
