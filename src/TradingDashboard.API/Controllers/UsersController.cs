using MediatR;
using Microsoft.AspNetCore.Mvc;
using TradingDashboard.Application.Features.Users.Commands.DeleteUser;
using TradingDashboard.Application.Features.Users.Commands.LoginUser;
using TradingDashboard.Application.Features.Users.Commands.RegisterUser;
using TradingDashboard.Application.Features.Users.Commands.UpdateUser;
using TradingDashboard.Application.Features.Users.Queries.GetUserById;

namespace TradingDashboard.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginUserCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetUserByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Update(Guid id, [FromBody] UpdateUserCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command with { Id = id }, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteUserCommand(id), cancellationToken);
        return NoContent();
    }
}
