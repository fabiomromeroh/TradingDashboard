using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TradingDashboard.Application.Abstractions.Services;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Infrastructure.Services.Identity;

public class JwtTokenService(
    IOptions<JwtSettingsOptions> jwtSettings,
    ILogger<JwtTokenService> logger) : IJwtTokenService
{
    private readonly JwtSettingsOptions _jwtSettings = jwtSettings.Value;
    private readonly ILogger<JwtTokenService> _logger = logger;
    private static readonly JwtSecurityTokenHandler TokenHandler = new();

    public string GenerateToken(User user)
    {
        // Validate SecretKey is configured
        if (string.IsNullOrWhiteSpace(_jwtSettings.SecretKey))
        {
            _logger.LogError("JWT SecretKey is not configured. Check App Service environment variables or appsettings.json");
            throw new InvalidOperationException("JWT SecretKey is not configured. Ensure JwtSettings__SecretKey environment variable is set in App Service.");
        }

        // Log where the configuration came from (for debugging)
        _logger.LogDebug("Generating JWT token with Issuer={Issuer}, Audience={Audience}, ExpiryMinutes={ExpiryMinutes}",
            _jwtSettings.Issuer, _jwtSettings.Audience, _jwtSettings.ExpiryMinutes);


        var keyBytes = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);

        if (keyBytes.Length < 32)
        {
            _logger.LogError("JWT SecretKey must be at least 32 bytes (256 bits) for HmacSha256.");
            throw new InvalidOperationException("JWT SecretKey is too short for HmacSha256.");
        }

        var key = new SymmetricSecurityKey(keyBytes);

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
            new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        //Todo- add user roles here
        //foreach (var role in user.Roles) // if you have roles
        //claims.Add(new Claim(ClaimTypes.Role, role));

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow, //protects against edge cases where server clocks drift slightly
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            signingCredentials: credentials);

        return TokenHandler.WriteToken(token);
    }
}

