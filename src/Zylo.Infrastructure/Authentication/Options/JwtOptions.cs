namespace Zylo.Infrastructure.Authentication.Options;

internal sealed class JwtOptions
{
    public const string ConfigurationSection = "Jwt";

    public string Secret { get; set; } = string.Empty;

    public string Issuer { get; set; } = string.Empty;

    public string Audience { get; set; } = string.Empty;

    public int ExpirationInMinutes { get; set; }
}
