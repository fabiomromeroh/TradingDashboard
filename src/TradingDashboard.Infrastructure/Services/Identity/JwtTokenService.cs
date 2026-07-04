using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TradingDashboard.Application.Common.Interfaces;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Infrastructure.Services.Identity;

public class JwtTokenService : IJwtTokenService
{
    private readonly JwtSettings _jwtSettings;
    private readonly ILogger<JwtTokenService> _logger;

    public JwtTokenService(
        IOptions<JwtSettings> jwtSettings,
        ILogger<JwtTokenService> logger)
    {
        _jwtSettings = jwtSettings.Value;
        _logger = logger;
    }

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

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
            new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

