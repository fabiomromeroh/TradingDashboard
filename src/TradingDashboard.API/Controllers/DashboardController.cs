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

    [HttpGet("summary")]
    public async Task<IActionResult> GetDashboardSummary([FromQuery] IReadOnlyCollection<Guid> accountIds, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetDashboardSummaryQuery(accountIds), cancellationToken);
        return result.ToActionResult();
    }
}

