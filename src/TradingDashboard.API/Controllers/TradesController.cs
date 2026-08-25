using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TradingDashboard.API.Extensions;
using TradingDashboard.Application.Features.Trades.Commands.CreateTrade;
using TradingDashboard.Application.Features.Trades.Commands.DeleteTrade;
using TradingDashboard.Application.Features.Trades.Queries.GetExecutionsByTradeId;
using TradingDashboard.Application.Features.Trades.Queries.GetTradeById;
using TradingDashboard.Application.Features.Trades.Queries.GetTradesByAccountId;
using TradingDashboard.Application.Features.Trades.Queries.GetTradesPaginated;

namespace TradingDashboard.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TradesController(IMediator mediator) : ControllerBase
{

    /// <summary>
    /// Handles HTTP GET requests to retrieve all trades.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult> Get(CancellationToken cancellationToken)
    {
        Guid userId = User.GetUserId();

        var result = await mediator.Send(new GetTradesByAccountIdQuery(userId), cancellationToken);

        return result.ToActionResult();
    }

    /// <summary>
    /// Retrieves trades with cursor-based pagination.
    /// </summary>
    /// <param name="pageSize">The number of trades to retrieve per page. Defaults to 20.</param>
    /// <param name="cursor">The cursor token from the previous result to paginate through results. Null for the first request.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A paginated result containing trades, next cursor, and a flag indicating if more data exists.</returns>
    [HttpGet("paginated")]
    public async Task<ActionResult> GetPaginated([FromQuery] int pageSize = 100, [FromQuery] string? cursor = null, CancellationToken cancellationToken = default)
    {
        Guid userId = User.GetUserId();

        if (pageSize < 1 || pageSize > 100)
        {
            return BadRequest("Page size must be between 1 and 100.");
        }

        var result = await mediator.Send(new GetTradesPaginatedQuery(userId, pageSize, cursor), cancellationToken);

        return result.ToActionResult();
    }

    /// <summary>
    /// Retrieves the trade with the specified unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the trade to retrieve.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>An <see cref="ActionResult"/> containing the trade data if found; otherwise, a not found result.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult> Get(Guid id, CancellationToken cancellationToken)
    {

        var result = await mediator.Send(new GetTradeByIdQuery(id), cancellationToken);

        return result.ToActionResult();
    }

    /// <summary>
    /// Creates a new trade using the specified command.
    /// </summary>
    /// <remarks>Returns a 200 OK response with the result if the trade is created successfully. The request
    /// may be canceled if the provided cancellation token is triggered.</remarks>
    /// <param name="createTradeCommand">The command containing the details required to create the trade. Cannot be null.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An ActionResult containing the result of the trade creation operation.</returns>
    [HttpPost]
    public async Task<IActionResult> Post(CreateTradeCommand createTradeCommand, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(createTradeCommand, cancellationToken);


        return result.ToActionResult(value => CreatedAtAction(nameof(CreateTradeCommand), value));
    }

    /// <summary>
    /// Deletes the trade with the specified identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the trade to delete.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A response indicating the result of the delete operation. Returns a 204 No Content status if the deletion is
    /// successful.</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {

        await mediator.Send(new DeleteTradeCommand() { Id = id }, cancellationToken);

        return NoContent();
    }

    [HttpGet("{id}/executions")]
    public async Task<ActionResult> GetExecutions(Guid id, CancellationToken cancellationToken)
    {
        var executions = await mediator.Send(new GetExecutionsByTradeIdQuery(id), cancellationToken);

        return executions.ToActionResult();
    }



}
