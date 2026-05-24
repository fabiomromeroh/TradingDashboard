using MediatR;
using Microsoft.AspNetCore.Mvc;
using TradingDashboard.API.Extensions;
using TradingDashboard.Application.Features.ImportSessions.Commands.CreateImportSession;
using TradingDashboard.Application.Features.ImportSessions.Dtos;
using TradingDashboard.Application.Features.ImportSessions.Queries.GetImportSessionById;
using TradingDashboard.Application.Features.ImportSessions.Queries.GetImportSessionsByAccount;

namespace TradingDashboard.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImportSessionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ImportSessionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateImportSessionCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return result.ToActionResult(dto => CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetImportSessionByIdQuery(id), cancellationToken);
        return result.ToActionResult();
    }

    [HttpGet("account/{accountId:guid}")]
    public async Task<ActionResult<IEnumerable<ImportSessionDto>>> GetByAccount(Guid accountId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetImportSessionsByAccountQuery(accountId), cancellationToken);
        return result.ToActionResult();
    }
}
