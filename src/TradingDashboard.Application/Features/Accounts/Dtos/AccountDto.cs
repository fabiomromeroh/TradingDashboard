namespace TradingDashboard.Application.Features.Accounts.Dtos;

public record AccountDto(
    Guid Id,
    string Name,
    string Currency,
    decimal InitialBalance,
    bool IsActive,
    Guid UserId,
    Guid BrokerId,
    DateTime CreatedAt);
