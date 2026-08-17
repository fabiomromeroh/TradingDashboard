using System.Linq.Expressions;

namespace TradingDashboard.Application.Abstractions.Services.Metric
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> ToExpression();
    }
}
