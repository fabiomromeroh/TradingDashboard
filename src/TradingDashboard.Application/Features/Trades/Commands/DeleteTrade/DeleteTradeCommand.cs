using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace TradingDashboard.Application.Features.Trades.Commands.DeleteTrade
{
    public record DeleteTradeCommand: IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}
