using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TradingDashboard.API.Extensions;
using TradingDashboard.Application.Features.Users.Commands.DeleteUser;
using TradingDashboard.Application.Features.Users.Commands.LoginUser;
using TradingDashboard.Application.Features.Users.Commands.LogoutUser;
using TradingDashboard.Application.Features.Users.Commands.RefreshTokenUser;
using TradingDashboard.Application.Features.Users.Commands.RegisterUser;
using TradingDashboard.Application.Features.Users.Commands.UpdateUser;
using TradingDashboard.Application.Features.Users.Queries.GetUserById;
using TradingDashboard.Application.Features.Users.Queries.GetUsers;

namespace TradingDashboard.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IHostEnvironment hostEnvironment;

    public UsersController(IMediator mediator, IHostEnvironment hostEnvironment)
    {
        _mediator = mediator;
        this.hostEnvironment = hostEnvironment;

    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return result.ToActionResult(value => CreatedAtAction(nameof(GetById), new { id = value.Id }, value));
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginUserCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        if (!result.IsSuccess) return result.ToActionResult();

        Response.Cookies.Append("refreshToken", result.Value!.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = !hostEnvironment.IsDevelopment(),
            SameSite = hostEnvironment.IsDevelopment() ? SameSiteMode.Lax : SameSiteMode.None,
            Expires = DateTimeOffset.UtcNow.AddDays(60),
            Path = "/api/users"
        });

        return result.ToActionResult(value => Ok(new { value.AccessToken, value.User }));
    }

    [HttpPost("logout")]
    public async Task<ActionResult> Logout(CancellationToken cancellationToken)
    {
        if (Request.Cookies.TryGetValue("refreshToken", out var rawRefreshToken))
        {
            var result = await _mediator.Send(new LogoutCommand(rawRefreshToken), cancellationToken);
            Response.Cookies.Delete("refreshToken", new CookieOptions { Path = "/api/users" });

            return result.ToActionResult();
        }

        return BadRequest();
    }

    [HttpPost("refresh")]
    public async Task<ActionResult> Refresh(CancellationToken ct)
    {
        if (!Request.Cookies.TryGetValue("refreshToken", out var rawToken))
            return Unauthorized();

        var result = await _mediator.Send(new RefreshTokenCommand(rawToken), ct);
        if (!result.IsSuccess) return result.ToActionResult();

        Response.Cookies.Append("refreshToken", result.Value!.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = !hostEnvironment.IsDevelopment(),
            SameSite = hostEnvironment.IsDevelopment() ? SameSiteMode.Lax : SameSiteMode.None,
            Expires = DateTimeOffset.UtcNow.AddDays(60),
            Path = "/api/users"
        });

        return Ok(new { accessToken = result.Value.AccessToken, user = result.Value.User });
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetUserByIdQuery(id), cancellationToken);
        return result.ToActionResult();
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    public async Task<ActionResult> Update(Guid id, [FromBody] UpdateUserCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command with { Id = id }, cancellationToken);
        return result.ToActionResult();
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteUserCommand(id), cancellationToken);
        return NoContent();
    }

    [HttpGet]
    //[Authorize]
    public async Task<ActionResult> Get(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetUsersQuery(), cancellationToken);

        return result.ToActionResult();

    }
}
