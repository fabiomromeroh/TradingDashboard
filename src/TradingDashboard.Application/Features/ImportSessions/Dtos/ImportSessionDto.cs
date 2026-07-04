namespace TradingDashboard.Application.Features.ImportSessions.Dtos;

public record ImportSessionDto(
    Guid Id,
    string? FileName,
    string? FileFormat,
    string BrokerName,
    string Status,
    int TotalRows,
    int SkippedRows,
    int ProcessedRows,
    bool IsRolledBack,
    string SourceType,
    DateTimeOffset? CompletedAt,
    DateTimeOffset? PeriodStart,
    DateTimeOffset? PeriodEnd,
    Guid AccountId,
    DateTimeOffset CreatedAt);
