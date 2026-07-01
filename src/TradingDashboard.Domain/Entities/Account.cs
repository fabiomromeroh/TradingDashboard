using TradingDashboard.Domain.Common;

namespace TradingDashboard.Domain.Entities;

public class Account : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string? Currency { get; private set; } = "USD";
    public decimal InitialBalance { get; private set; } = 0;
    public bool IsActive { get; private set; } = true;

    public Guid UserId { get; private set; }

    public Guid BrokerId { get; private set; }

    public User User { get; private set; } = null!;
    public Broker Broker { get; private set; } = null!;

    public IReadOnlyCollection<Trade> Trades => _trades.AsReadOnly();
    private readonly List<Trade> _trades = [];

    private readonly List<ImportSession> _importSessions = [];
    public IReadOnlyCollection<ImportSession> ImportSessions => _importSessions.AsReadOnly();

    private Account() { }

    public static Account Create(string name, Guid userId, Guid brokerId, decimal initialBalance = default, string? currency = default)
    {
        return new()
        {
            Name = name,
            Currency = currency,
            InitialBalance = initialBalance,
            UserId = userId,
            BrokerId = brokerId
        };
    }

    public void Update(string name, string currency, decimal initialBalance)
    {
        Name = name;
        Currency = currency;
        InitialBalance = initialBalance;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
