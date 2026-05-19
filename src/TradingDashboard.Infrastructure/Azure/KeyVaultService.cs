using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace TradingDashboard.Infrastructure.Azure;

/// <summary>
/// Service for retrieving secrets from Azure Key Vault with optional caching.
/// </summary>
public interface IKeyVaultService
{
    /// <summary>
    /// Retrieves a secret value from Key Vault.
    /// </summary>
    /// <param name="secretName">The name of the secret to retrieve.</param>
    /// <returns>The secret value, or null if not found.</returns>
    Task<string?> GetSecretAsync(string secretName);
}

public class KeyVaultService : IKeyVaultService
{
    private readonly SecretClient? _secretClient;
    private readonly IMemoryCache _cache;
    private readonly ILogger<KeyVaultService> _logger;
    private readonly KeyVaultSettings _settings;
    private const int CacheDurationMinutes = 60;

    public KeyVaultService(
        IMemoryCache cache,
        ILogger<KeyVaultService> logger,
        KeyVaultSettings settings)
    {
        _cache = cache;
        _logger = logger;
        _settings = settings;

        if (_settings.Enabled && !string.IsNullOrWhiteSpace(_settings.VaultUri))
        {
            try
            {
                _secretClient = new SecretClient(
                    new Uri(_settings.VaultUri),
                    new DefaultAzureCredential());

                _logger.LogInformation("Key Vault service initialized successfully with URI: {VaultUri}", _settings.VaultUri);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to initialize Key Vault service");
                throw;
            }
        }
        else
        {
            _logger.LogInformation("Key Vault is disabled or not configured");
        }
    }

    public async Task<string?> GetSecretAsync(string secretName)
    {
        if (_secretClient == null)
        {
            _logger.LogWarning("Key Vault client is not initialized. Cannot retrieve secret: {SecretName}", secretName);
            return null;
        }

        var cacheKey = $"keyvault_{secretName}";

        if (_cache.TryGetValue(cacheKey, out string? cachedValue))
        {
            _logger.LogDebug("Retrieved secret from cache: {SecretName}", secretName);
            return cachedValue;
        }

        try
        {
            KeyVaultSecret secret = await _secretClient.GetSecretAsync(secretName);
            var secretValue = secret.Value;

            _cache.Set(cacheKey, secretValue, TimeSpan.FromMinutes(CacheDurationMinutes));

            _logger.LogInformation("Retrieved secret from Key Vault: {SecretName}", secretName);
            return secretValue;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve secret from Key Vault: {SecretName}", secretName);
            throw;
        }
    }
}
