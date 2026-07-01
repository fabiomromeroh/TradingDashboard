using MediatR;
using TradingDashboard.Application.Common;
using TradingDashboard.Domain.Enums;

namespace TradingDashboard.Application.Features.Trades.Commands.CreateTrade;

public record CreateTradeCommand : IRequest<Result<Guid>>
{
    public string Symbol { get; init; } = string.Empty;
    public decimal EntryPrice { get; init; }
    public decimal Quantity { get; init; }
    /// <summary>
    /// Gets the direction of the trade (buy or sell).
    /// </summary>
    public TradeDirection Direction { get; init; }

    public Guid AccountId { get; set; }
}
