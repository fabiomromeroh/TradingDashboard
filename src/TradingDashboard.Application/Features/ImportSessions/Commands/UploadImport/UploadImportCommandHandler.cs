using MediatR;
using System.Net;
using TradingDashboard.Application.Abstractions.Repositories;
using TradingDashboard.Application.Abstractions.Services.Import;
using TradingDashboard.Application.Common;
using TradingDashboard.Application.Features.ImportSessions.Dtos;

namespace TradingDashboard.Application.Features.ImportSessions.Commands.UploadImport
{
    public class UploadImportCommandHandler : IRequestHandler<UploadImportCommand, Result<ImportPreviewtDto>>
    {
        private readonly IBrokerParserFactory brokerParserFactory;
        private readonly IExecutionRepository executionRepository;

        public UploadImportCommandHandler(IBrokerParserFactory brokerParserFactory, IExecutionRepository executionRepository)
        {
            this.brokerParserFactory = brokerParserFactory;
            this.executionRepository = executionRepository;
        }
        public async Task<Result<ImportPreviewtDto>> Handle(UploadImportCommand command, CancellationToken ct)
        {
            //1 — Check broker is supported
            if (!brokerParserFactory.SupportedBrokers.Contains(command.BrokerName))
                return Result<ImportPreviewtDto>.Failure(
                    new Error("UnsupportedBroker",
                        $"'{command.BrokerName}' is not supported. " +
                        $"Supported brokers: {string.Join(", ", brokerParserFactory.SupportedBrokers)}"),
                    HttpStatusCode.BadRequest);

            // 2 — Parse the file
            var parser = brokerParserFactory.GetParser(command.BrokerName);
            var parsed = parser.Parse(command.FileContent);

            // 3 — Reject if the file format is completely wrong
            if (parsed.ParseErrors.Any())
            {
                return Result<ImportPreviewtDto>.Failure(
                    new Error("InvalidFormat", $"File does not match the expected {command.BrokerName} format. " +
                    $"First error: {parsed.ParseErrors.First()}"),
                HttpStatusCode.BadRequest);
            }

            if (!parsed.Rows.Any())
                return Result<ImportPreviewtDto>.Failure(
                    new Error("EmptyFile", "No execution rows were found in this file"),
                    HttpStatusCode.BadRequest);

            // 4 — Check which rows are duplicates
            var brokerExecutionIds = parsed.Rows
                .Select(r => r.BrokerExecutionId)
                .ToList();

            var existingIds = await executionRepository
                .GetExistingBrokerExecutionIdsAsync(brokerExecutionIds, command.AccountId, ct);

            if (existingIds.Count == brokerExecutionIds.Count)
            {
                return Result<ImportPreviewtDto>.Failure(
                    new Error("DuplicatedFile", "This file was uploaded already or all trades in this file already exist in this account."),
                    HttpStatusCode.BadRequest);
            }


            // 5 — Build preview rows
            var previewRows = parsed.Rows
            .Select(row => new PreviewRowDto(
                RowNumber: row.RowNumber,
                Symbol: row.Symbol,
                Description: row.Description,
                Side: row.Side,
                Quantity: row.Quantity,
                Price: row.Price,
                Commission: row.Commission,
                Exchange: row.Exchange,
                OrderType: row.OrderType,
                ExecutedAt: row.ExecutedAt,
                IsDuplicate: existingIds.Contains(row.BrokerExecutionId),
                ParseError: null,
                BrokerExecutionId: row.BrokerExecutionId,
                BrokerOrderId: row.BrokerOrderId,
                BrokerTradeId: row.BrokerTradeId
            ))
            .ToList();

            // 6 — Return preview — nothing saved yet
            return Result<ImportPreviewtDto>.Success(new ImportPreviewtDto(
                FileName: command.FileName,
                BrokerName: command.BrokerName,
                AccountId: command.AccountId,
                TotalRows: parsed.Rows.Count,
                NewRows: previewRows.Count(r => !r.IsDuplicate && r.ParseError is null),
                DuplicateRows: previewRows.Count(r => r.IsDuplicate),
                InvalidRows: parsed.ParseErrors.Count,
                Rows: previewRows
            ));
        }
    }
}
