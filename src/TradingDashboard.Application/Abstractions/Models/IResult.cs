using System.Net;
using TradingDashboard.Application.Common;

namespace TradingDashboard.Application.Abstractions.Models
{
    public interface IResult
    {

        bool IsSuccess { get; }
        bool IsFailure { get; }
        IReadOnlyList<Error> Errors { get; }
        HttpStatusCode StatusCode { get; }
    }
}
