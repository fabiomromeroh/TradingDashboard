using LinqKit;
using System.Linq.Expressions;
using TradingDashboard.Application.Common.Models;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Application.Abstractions.Services.Metric.Specifications
{
    public class MetricFilterSpecification(ConfigFilter? queryFilter) : ISpecification<Trade>
    {
        private readonly ConfigFilter _filter = queryFilter ?? new ConfigFilter([]);

        public Expression<Func<Trade, bool>> ToExpression()
        {
            var predicate = PredicateBuilder.New<Trade>(true);

            if (_filter.AccountIds is { Count: > 0 } accountIds)
                predicate = predicate.And(t => accountIds.Contains(t.AccountId));

            //if (_filter.DateFrom is { } from)
            //    predicate = predicate.And(t => t.OpenedAt >= from.ToDateTime(TimeOnly.MinValue));

            //if (_filter.DateTo is { } to)
            //    predicate = predicate.And(t => t.OpenedAt <= to.ToDateTime(TimeOnly.MaxValue));

            return predicate;
        }
    }
}
