using MediatR;
using System.Data;
using TradingDashboard.Application.Abstractions;
using TradingDashboard.Application.Abstractions.Repositories;
using TradingDashboard.Application.Abstractions.Services.Import;
using TradingDashboard.Application.Common.Exceptions;
using TradingDashboard.Application.Common.Models;
using TradingDashboard.Domain.Entities;
using TradingDashboard.Domain.Enums;

namespace TradingDashboard.Application.Features.ImportSessions.Commands.ConfirmImport
{
    public class ConfirmImportCommandHandler(IUnitOfWork unitOfWork, IImportSessionRepository importSessionRepository, IExecutionRepository executionRepository, IImportService importService) : IRequestHandler<ConfirmImportCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(ConfirmImportCommand command, CancellationToken ct)
        {

            var orderedRows = command.Rows
                                .Where(r => !r.IsDuplicate)
                                .OrderBy(x => x.ExecutedAt)
                                .ThenBy(r => r.BrokerExecutionId)
                                .ToList();

            var startPeriod = orderedRows.Min(x => x.ExecutedAt);
            var endPeriod = orderedRows.Max(x => x.ExecutedAt);

            //create Import Session
            var importSession = ImportSession.Create(command.AccountId, command.BrokerName, ImportSourceType.FileUpload, command.FileName);
            importSession.Complete(command.TotalRows, command.DuplicateRows, startPeriod, endPeriod);


            return await unitOfWork.ExecuteInTransactionAsync(async ct =>
            {

                await importSessionRepository.AddAsync(importSession, ct);

                //Process and save executions 
                foreach (var row in orderedRows)
                {
                    var execution = Execution.Create(
                        command.AccountId,
                        row.Symbol,
                        row.Price,
                        row.Quantity,
                        row.Side.ToEnum(),
                        row.ExecutedAt,
                        row.Commission,
                        row.BrokerExecutionId,
                        row.BrokerOrderId,
                        importSession.Id,
                        row.Exchange,
                        row.OrderType

                        );

                    await executionRepository.AddAsync(execution, ct);

                }

                await unitOfWork.SaveChangesAsync(ct);

                var impactedSymbols = orderedRows
                      .Select(x => x.Symbol)
                      .Distinct(StringComparer.OrdinalIgnoreCase)
                      .ToArray();

                await importService.RebuildTradesAsync(command.AccountId, impactedSymbols, ct);



                return Result<Guid>.Success(importSession.Id);

            }, ct);


        }

    }
}
