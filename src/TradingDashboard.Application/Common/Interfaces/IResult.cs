using System.Net;

namespace TradingDashboard.Application.Common.Interfaces
{
  public interface IResult {

        bool IsSuccess { get; }
        bool IsFailure { get; }
        IReadOnlyList<Error> Errors { get; }
        HttpStatusCode StatusCode { get; }
    }
}
