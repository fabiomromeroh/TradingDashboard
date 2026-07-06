using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TradingDashboard.API.Extensions;
using TradingDashboard.Application.Features.Trades.Commands.CreateTrade;
using TradingDashboard.Application.Features.Trades.Commands.DeleteTrade;
using TradingDashboard.Application.Features.Trades.Queries.GetAllTrades;
using TradingDashboard.Application.Features.Trades.Queries.GetExecutionsByTradeId;
using TradingDashboard.Application.Features.Trades.Queries.GetTradeById;

namespace TradingDashboard.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TradesController : ControllerBase
{
    private readonly IMediator mediator;

    public TradesController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    /// <summary>
    /// Handles HTTP GET requests to retrieve all trades.
    /// </summary>
    /// <param name="accountIds"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("accounts")]
    public async Task<ActionResult> GetByAccountId([FromBody] List<Guid> accountIds, CancellationToken cancellationToken)
    {

        var result = await mediator.Send(new GetTradesByAccountIdQuery(accountIds), cancellationToken);

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
