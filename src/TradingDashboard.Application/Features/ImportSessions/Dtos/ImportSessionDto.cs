namespace TradingDashboard.Application.Features.ImportSessions.Dtos;

public record ImportSessionDto(
    Guid Id,
    string FileName,
    string Status,
    int TotalTrades,
    int ImportedTrades,
    string? ErrorMessage,
    DateTime? CompletedAt,
    Guid AccountId,
    DateTime CreatedAt);
