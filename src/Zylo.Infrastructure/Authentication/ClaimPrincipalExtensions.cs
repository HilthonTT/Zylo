using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace Zylo.Infrastructure.Authentication;

public static class ClaimPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal? principal)
    {
        string? userId = principal?.FindFirstValue(ClaimTypes.NameIdentifier);

        return Guid.TryParse(userId, out Guid parsedUserId) ? 
            parsedUserId :
            throw new ApplicationException("User identifier is unavailable");
    }
}
