namespace TradingDashboard.Application.Features.Accounts.Dtos;

public record AccountDto(
    Guid Id,
    string Name,
    string ImportSourceType,
    bool IsActive,
    string BrokerName,
    Guid UserId,
    Guid BrokerId,
    DateTimeOffset CreatedAt)
{
    public object? BrokerCredentials { get; set; }
    public int TradesCount { get; set; } = 0;

};
