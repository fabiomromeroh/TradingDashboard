using TradingDashboard.Application.Abstractions.Services.Metric;
using TradingDashboard.Application.Common.Models;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Application.Abstractions.Services.Trades
{
    public interface ITradeQueryService
    {
        Task<TradeAggregateDto> GetTradeAggregatesAsync(Guid userId, ISpecification<Trade> spec, CancellationToken ct);
        Task<IEnumerable<Trade>> GetTradesAsync(Guid userId, ISpecification<Trade> spec, CancellationToken ct);

        Task<IEnumerable<Trade>> GetTradesByAccountId(List<Guid> accountIds, CancellationToken cancellationToken);

        public Task<Trade?> GetTradeAsync(Guid id, CancellationToken cancellationToken);

        Task<PaginatedResult<Trade>> GetTradesPaginatedAsync(Guid userId, ISpecification<Trade> spec, int pageSize, string? cursor, CancellationToken ct);

    }
}
