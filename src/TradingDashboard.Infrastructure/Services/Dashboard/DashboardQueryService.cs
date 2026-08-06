using Microsoft.EntityFrameworkCore;
using TradingDashboard.Application.Abstractions.Services.Dashboard;
using TradingDashboard.Application.Common.Models;
using TradingDashboard.Application.Features.Dashboard.Dtos;
using TradingDashboard.Infrastructure.Persistence;

namespace TradingDashboard.Infrastructure.Services.Dashboard
{
    public class DashboardQueryService : IDashboardQueryService
    {
        private readonly AppDbContext appDbContext;

        public DashboardQueryService(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public async Task<DashboardSummaryDto> GetDashboardSummaryAsync(QueryFilter filter, CancellationToken ct)
        {
            var result = await appDbContext.Trades
                .Where(x => x.Account.IsActive && filter.AccountIds.Contains(x.AccountId))
                .GroupBy(x => 1)
                .Select(x => new DashboardSummaryDto
                {
                    NetPnl = x.Sum(x => x.NetReturn ?? 0),
                    TradeCount = x.Count(),
                    WinCount = x.Count(x => x.NetReturn > 0),
                    LossCount = x.Count(x => x.NetReturn < 0),
                    AverageWin = x.Where(x => x.NetReturn > 0).Average(x => x.NetReturn ?? 0),
                    AverageLoss = x.Where(x => x.NetReturn < 0).Average(x => x.NetReturn ?? 0)

                }).FirstOrDefaultAsync(ct);

            return result ?? new DashboardSummaryDto();
        }


    }
}
