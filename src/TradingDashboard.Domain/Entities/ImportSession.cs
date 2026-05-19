using System.Reflection.Metadata;
using TradingDashboard.Domain.Common;
using TradingDashboard.Domain.Enums;

namespace TradingDashboard.Domain.Entities;

public class ImportSession : BaseEntity
{
    public string FileName { get; private set; } = string.Empty;
    public string? FileHash { get; private set; }  // SHA-256, duplicate
    public string? FileFormat { get; private set; }     // "CSV", "PDF", "XLSX"
    public ImportSourceType SourceType { get; private set; }
    public string? StoragePath { get; private set; } // blob path or local path

    // ── Common to ALL source types ──────────────────────────────
    public string BrokerName { get; private set; }      // "IBKR", "Alpaca", "TD"
    public ImportSessionStatus Status { get; private set; } = ImportSessionStatus.Pending; // Pending, Processing, Completed, Failed
    public DateTimeOffset StartedAt { get; private set; }
    public DateTimeOffset? CompletedAt { get; private set; }
    public int TotalRows { get; private set; }
    public int ProcessedRows { get; private set; }
    public int FailedRows { get; private set; }
    public string? ErrorSummary { get; private set; }
    public DateTimeOffset? PeriodStart { get; private set; }
    public DateTimeOffset? PeriodEnd { get; private set; }

    // ── Executions created in this session ─────────────────────
    private readonly List<Execution> _executions = new();
    public IReadOnlyList<Execution> Executions => _executions;

    public Guid AccountId { get; private set; }
    public Account? Account { get; private set; }

    private ImportSession() { }

    public static ImportSession Create(string fileName, Guid accountId)
    {
        return new ImportSession
        {
            FileName = fileName,
            AccountId = accountId
        };
    }

    public void Complete(int totalRows, int importedRows)
    {
        TotalRows = totalRows;
        ProcessedRows = importedRows;
        Status = ImportSessionStatus.Completed;
        CompletedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Fail(string errorMessage)
    {
        ErrorSummary = errorMessage;
        Status = ImportSessionStatus.Failed;
        CompletedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}
