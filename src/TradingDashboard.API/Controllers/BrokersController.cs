using MediatR;
using Microsoft.AspNetCore.Mvc;
using TradingDashboard.API.Extensions;
using TradingDashboard.Application.Features.ImportSessions.Queries.GetBrokers;

namespace TradingDashboard.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BrokersController : ControllerBase
    {
        private readonly IMediator mediator;

        public BrokersController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Get(CancellationToken cancellationToken)
        {

            var result = await mediator.Send(new GetBrokersQuery(), cancellationToken);

            return result.ToActionResult();
        }
    }
}
