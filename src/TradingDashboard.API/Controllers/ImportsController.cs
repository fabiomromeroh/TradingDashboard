using MediatR;
using Microsoft.AspNetCore.Mvc;
using TradingDashboard.API.Extensions;
using TradingDashboard.Application.Features.ImportSessions.Commands.ConfirmImport;
using TradingDashboard.Application.Features.ImportSessions.Commands.UploadImport;
using TradingDashboard.Application.Features.ImportSessions.Dtos;
using TradingDashboard.Application.Features.ImportSessions.Queries.GetImportSessionById;
using TradingDashboard.Application.Features.ImportSessions.Queries.GetImportSessionsByAccount;

namespace TradingDashboard.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImportsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ImportsController(IMediator mediator)
    {
        _mediator = mediator;
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

    [HttpPost("upload")]
    [RequestSizeLimit(10 * 1024 * 1024)] //10 MB
    public async Task<ActionResult<ImportPreviewtDto>> Upload(IFormFile file, [FromForm] string brokerName, [FromForm] Guid accountId, CancellationToken ct)
    {
        await using var stream = file.OpenReadStream();
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream, ct);
        var fileContent = memoryStream.ToArray();

        var command = new UploadImportCommand(FileContent: fileContent, FileName: file.FileName, BrokerName: brokerName, AccountId: accountId);
        var result = await _mediator.Send(command, ct);

        return result.ToActionResult();
    }

    [HttpPost("confirm")]
    public async Task<ActionResult<string>> Confirm([FromBody] ConfirmImportCommand command, CancellationToken ct)
    {

        var result = await _mediator.Send(command, ct);

        return result.ToActionResult();
    }


}
