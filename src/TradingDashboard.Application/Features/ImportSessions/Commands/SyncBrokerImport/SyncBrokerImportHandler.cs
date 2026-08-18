using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;
using TradingDashboard.Application.Abstractions;
using TradingDashboard.Application.Abstractions.Repositories;
using TradingDashboard.Application.Abstractions.Services.BrokerSync;
using TradingDashboard.Application.Abstractions.Services.Import;
using TradingDashboard.Application.Common.Exceptions;
using TradingDashboard.Application.Common.Models;
using TradingDashboard.Application.Features.ImportSessions.Dtos;
using TradingDashboard.Domain.Entities;
using TradingDashboard.Domain.Enums;

namespace TradingDashboard.Application.Features.ImportSessions.Commands.SyncBrokerImport
{
    public class SyncBrokerImportHandler(IBrokerSyncFactory factory,
        IBrokerAccountCredentialService brokerAccountCredentialService,
        IExecutionRepository executionRepository,
        ILogger<SyncBrokerImportHandler> logger,
        IAccountRepository accountRepository,
        IImportSessionRepository importSessionRepository,
        IImportService importService,
        IUnitOfWork unitOfWork) : IRequestHandler<SyncBrokerImportCommand, Result<SyncBrokerDto>>
    {
        public async Task<Result<SyncBrokerDto>> Handle(SyncBrokerImportCommand command, CancellationToken cancellationToken)
        {

            var account = await accountRepository.GetByIdAsync(command.AccountId, cancellationToken);
            if (account is null) return Result<SyncBrokerDto>.NotFound(nameof(Account), command.AccountId);

            if (!factory.SupportedBrokers.Contains(account.Broker.Name))
                return Result<SyncBrokerDto>.Failure(
                        new Error("UnsupportedBroker",
                            $"'{account.Broker.Name}' is not supported. " +
                            $"Supported brokers: {string.Join(", ", factory.SupportedBrokers)}"),
                        HttpStatusCode.BadRequest);

            //get broker's credentials 
            var credentials = await brokerAccountCredentialService.GetAsync<BrokerCredentials>(account.Id, cancellationToken);

            if (credentials is null)
                return Result<SyncBrokerDto>.Failure(
                        new Error("MissingCredentials",
                            $"No credentials found for account '{account.Name}'"),
                        HttpStatusCode.BadRequest);

            //TODO: Add a command property to define data range - All Trades = 365 days back and Last Trades = only from last sync date. 

            DateOnly fromDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-100));
            DateOnly toDate = DateOnly.FromDateTime(DateTime.Today);

            //get the broker sync service from the factory
            var syncService = factory.GetSyncService(account.Broker.Name);

            var result = await syncService.SyncAsync(new BrokerSyncRequest(credentials, fromDate, toDate), cancellationToken);

            if (!result.IsSuccess)
            {
                logger.LogError("Broker sync failed for account {AccountId}: {Code} {Message}", command.AccountId, result.ErrorCode, result.ErrorMessage);
                return Result<SyncBrokerDto>.Failure(new Error("502", result.ErrorMessage ?? "Broker sync failed"), HttpStatusCode.BadGateway);
            }

            // 4 — Check which rows are duplicates
            var brokerExecutionIds = result.Executions
                .Select(r => r.BrokerExecutionId)
                .ToList();

            var existingIds = await executionRepository
                .GetExistingBrokerExecutionIdsAsync(brokerExecutionIds, command.AccountId, cancellationToken);

            if (existingIds.Count == brokerExecutionIds.Count)
            {
                return Result<SyncBrokerDto>.Success(new SyncBrokerDto());
            }

            var orderedExecutions = result.Executions.Where(x => !existingIds.Contains(x.BrokerExecutionId)).OrderBy(x => x.ExecutedAt)
                .ThenBy(x => x.BrokerExecutionId)
                .ToList();

            var startPeriod = orderedExecutions.Min(x => x.ExecutedAt);
            var endPeriod = orderedExecutions.Max(x => x.ExecutedAt);
            var totalRows = orderedExecutions.Count;

            //create Import Session
            var importSession = ImportSession.Create(command.AccountId, account.Broker.Name, ImportSourceType.BrokerSync);
            importSession.Complete(totalRows, 0, startPeriod, endPeriod);

            return await unitOfWork.ExecuteInTransactionAsync(async ct =>
            {

                await importSessionRepository.AddAsync(importSession, ct);

                //Process and save executions 
                foreach (var row in orderedExecutions)
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

                    await executionRepository.AddAsync(execution, cancellationToken);

                }

                await unitOfWork.SaveChangesAsync(cancellationToken);

                //rebuild trades
                var impactedSymbols = orderedExecutions
                      .Select(x => x.Symbol)
                      .Distinct(StringComparer.OrdinalIgnoreCase)
                      .ToArray();

                var newTrades = await importService.RebuildTradesAsync(command.AccountId, impactedSymbols, cancellationToken);

                return Result<SyncBrokerDto>.Success(new SyncBrokerDto() { NewTrades = newTrades });

            }, cancellationToken);
        }
    }
}
