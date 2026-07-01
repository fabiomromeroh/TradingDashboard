namespace TradingDashboard.Application.Features.Accounts.Dtos;

public record AccountDto(
    Guid Id,
    string Name,
    string? Currency,
    decimal InitialBalance,
    bool IsActive,
    string BrokerName,
    Guid UserId,
    Guid BrokerId,
    DateTimeOffset CreatedAt);
