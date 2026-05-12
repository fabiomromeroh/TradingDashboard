using System;
using System.Collections.Generic;
using System.Text;

namespace TradingDashboard.Application.Features.Trades.Dtos
{
    public record TradeDto(
            Guid Id,
            string Symbol,
            decimal EntryPrice,
            decimal Quantity,
            string Direction,
            string Status,
            DateTime OpenedAt
            );
}
