namespace TradingDashboard.Infrastructure.Services.Identity
{
    /// <summary>
    /// JWT Settings configuration class for dependency injection
    /// </summary>
    public class JwtSettingsOptions
    {
        public string SecretKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = "TradingDashboard";
        public string Audience { get; set; } = "TradingDashboard";
        public int ExpiryMinutes { get; set; } = 60;
    }
}
