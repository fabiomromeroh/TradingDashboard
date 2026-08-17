using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TradingDashboard.API.Extensions;
using TradingDashboard.Application.Features.Dashboard.Queries;

namespace TradingDashboard.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IMediator _mediator;

    public DashboardController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("metric")]
    public async Task<IActionResult> GetMetric([FromQuery] string metricType, CancellationToken cancellationToken)
    {
        Guid userId = HttpContext.User.GetUserId();

        var result = await _mediator.Send(new GetMetricQuery(userId, metricType), cancellationToken);
        return result.ToActionResult();
    }
}

