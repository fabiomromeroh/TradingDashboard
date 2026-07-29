using TradingDashboard.Domain.Common;
using TradingDashboard.Domain.Enums;

namespace TradingDashboard.Domain.Entities;

public class ImportSession : BaseEntity
{
    public Guid AccountId { get; private set; }
    public required string BrokerName { get; set; }
    public string? FileName { get; private set; } = string.Empty;
    public string? FileHash { get; private set; }  // SHA-256, duplicate
    public string? FileFormat { get; private set; }     // "CSV", "PDF", "XLSX"
    public ImportSourceType SourceType { get; private set; }

    // ── Common to ALL source types ──────────────────────────────
    public ImportSessionStatus Status { get; private set; } = ImportSessionStatus.Completed; // Completed, RolledBack
    public DateTimeOffset? CompletedAt { get; private set; }
    public int TotalRows { get; private set; }
    public int ProcessedRows { get; private set; }
    public int SkippedRows { get; private set; }
    public bool IsRolledBack { get; private set; }
    public DateTimeOffset? PeriodStart { get; private set; }
    public DateTimeOffset? PeriodEnd { get; private set; }

    // ── Executions created in this session ─────────────────────
    private readonly List<Execution> _executions = [];
    public IReadOnlyList<Execution> Executions => _executions;

    public Account? Account { get; private set; }

    private ImportSession() { }

    public static ImportSession Create(Guid accountId, string brokerName, ImportSourceType sourceType, string? fileName = default)
    {
        return new()
        {
            AccountId = accountId,
            BrokerName = brokerName,
            SourceType = sourceType,
            FileName = fileName,
        };
    }

    public void Complete(int total, int skipped,
                         DateTimeOffset periodStart, DateTimeOffset periodEnd)
    {
        TotalRows = total;
        ProcessedRows = total - skipped;
        SkippedRows = skipped;
        PeriodStart = periodStart;
        PeriodEnd = periodEnd;
        Status = ImportSessionStatus.Completed;
        CompletedAt = DateTimeOffset.UtcNow;
    }

    public void MarkAsRolledBack()
    {
        IsRolledBack = true;
        Status = ImportSessionStatus.RolledBack;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
