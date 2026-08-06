using MediatR;
using TradingDashboard.Application.Common.Models;

namespace TradingDashboard.Application.Features.Trades.Commands.DeleteTrade
{
    public record DeleteTradeCommand: IRequest<Result>
    {
        public Guid Id { get; set; }
    }
}
