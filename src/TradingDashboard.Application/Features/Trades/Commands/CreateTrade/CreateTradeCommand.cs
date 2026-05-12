using MediatR;
using TradingDashboard.Application.Features.Trades.Dtos;
using TradingDashboard.Domain.Enums;

namespace TradingDashboard.Application.Features.Trades.Commands.CreateTrade;

public record CreateTradeCommand : IRequest<Guid>
{
    public string Symbol { get; init; } = string.Empty;
    public decimal EntryPrice { get; init; }
    public decimal Quantity { get; init; }
    /// <summary>
    /// Gets the direction of the trade (buy or sell).
    /// </summary>
    public TradeDirection Direction { get; init; }
}
