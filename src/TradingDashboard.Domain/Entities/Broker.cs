using TradingDashboard.Domain.Common;

namespace TradingDashboard.Domain.Entities;

public class Broker : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string DisplayName { get; private set; } = string.Empty;
    public string? Website { get; private set; }
    public string? SupportedImportFormat { get; private set; }

    public IReadOnlyCollection<Account> Accounts => _accounts.AsReadOnly();
    private readonly List<Account> _accounts = [];

    private Broker() { }

    public static Broker Create(string name, string displayName, string? website = null, string? supportedImportFormat = null)
    {
        return new Broker
        {
            Name = name,
            DisplayName = displayName,
            Website = website,
            SupportedImportFormat = supportedImportFormat
        };
    }

    public void Update(string name, string displayName, string? website, string? supportedImportFormat)
    {
        Name = name;
        DisplayName = displayName;
        Website = website;
        SupportedImportFormat = supportedImportFormat;
        UpdatedAt = DateTime.UtcNow;
    }
}
