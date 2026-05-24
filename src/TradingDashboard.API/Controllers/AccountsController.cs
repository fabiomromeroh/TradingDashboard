using MediatR;
using Microsoft.AspNetCore.Mvc;
using TradingDashboard.Application.Features.Accounts.Commands.CreateAccount;
using TradingDashboard.Application.Features.Accounts.Commands.DeleteAccount;
using TradingDashboard.Application.Features.Accounts.Commands.UpdateAccount;
using TradingDashboard.Application.Features.Accounts.Queries.GetAccountById;
using TradingDashboard.Application.Features.Accounts.Queries.GetAccountsByUser;

namespace TradingDashboard.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAccountCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Value }, result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAccountByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpGet("user/{userId:guid}")]
    public async Task<ActionResult> GetByUser(Guid userId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAccountsByUserQuery(userId), cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Update(Guid id, [FromBody] UpdateAccountCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command with { Id = id }, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteAccountCommand(id), cancellationToken);
        return NoContent();
    }
}
