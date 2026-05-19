namespace TradingDashboard.Infrastructure.Azure;

/// <summary>
/// Configuration settings for Azure Key Vault integration.
/// </summary>
public class KeyVaultSettings
{
    /// <summary>
    /// The URI of the Azure Key Vault (e.g., https://mykeyvault.vault.azure.net/).
    /// </summary>
    public string? VaultUri { get; set; }

    /// <summary>
    /// Indicates whether Key Vault is enabled. If false, secrets are read from configuration.
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// The name of the Key Vault secret containing the JWT secret key.
    /// </summary>
    public string JwtSecretKeyName { get; set; } = "JwtSecretKey";
}
