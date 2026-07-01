using MediatR;
using Microsoft.AspNetCore.Mvc;
using TradingDashboard.API.Extensions;
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
        Guid userId = Guid.Parse("dd7e0338-d43d-4f24-a274-22bbf194dc3e");

        var enrichedCommand = command with { UserId = userId };

        var result = await _mediator.Send(enrichedCommand, cancellationToken);
        return result.ToActionResult(value => CreatedAtAction(nameof(GetById), new { id = value.Id }, value));

    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAccountByIdQuery(id), cancellationToken);
        return result.ToActionResult();
    }

    [HttpGet("user/{userId:guid}")]
    public async Task<ActionResult> GetByUser(Guid userId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAccountsByUserQuery(userId), cancellationToken);
        return result.ToActionResult();
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Update(Guid id, [FromBody] UpdateAccountCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command with { Id = id }, cancellationToken);
        return result.ToActionResult();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteAccountCommand(id), cancellationToken);
        return NoContent();
    }
}
