using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;
using TradingDashboard.Application.Abstractions.Repositories;
using TradingDashboard.Application.Abstractions.Services.BrokerSync;
using TradingDashboard.Application.Abstractions.Services.Import;
using TradingDashboard.Application.Common;
using TradingDashboard.Application.Common.Exceptions;
using TradingDashboard.Application.Features.ImportSessions.Dtos;
using TradingDashboard.Application.Interfaces;
using TradingDashboard.Domain.Entities;
using TradingDashboard.Domain.Enums;

namespace TradingDashboard.Application.Features.ImportSessions.Commands.SyncBrokerImport
{
    public class SyncBrokerImportHandler : IRequestHandler<SyncBrokerImportCommand, Result<SyncBrokerDto>>
    {
        private readonly IBrokerSyncFactory _brokerSyncFactory;
        private readonly IBrokerAccountCredentialService _brokerAccountCredentialService;
        private readonly ILogger<SyncBrokerImportHandler> _logger;
        private readonly IAccountRepository _accountRepository;
        private readonly IExecutionRepository _executionRepository;
        private readonly IImportSessionRepository _importSessionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImportService _importService;

        public SyncBrokerImportHandler(IBrokerSyncFactory factory,
            IBrokerAccountCredentialService brokerAccountCredentialService,
            IExecutionRepository executionRepository,
            ILogger<SyncBrokerImportHandler> logger,
            IAccountRepository accountRepository,
            IImportSessionRepository importSessionRepository,
            IImportService importService,
            IUnitOfWork unitOfWork)
        {
            _brokerSyncFactory = factory;
            _brokerAccountCredentialService = brokerAccountCredentialService;
            _logger = logger;
            _accountRepository = accountRepository;
            _importSessionRepository = importSessionRepository;
            _executionRepository = executionRepository;
            _importService = importService;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<SyncBrokerDto>> Handle(SyncBrokerImportCommand command, CancellationToken cancellationToken)
        {

            var account = await _accountRepository.GetByIdAsync(command.AccountId, cancellationToken);
            if (account is null) return Result<SyncBrokerDto>.NotFound(nameof(Account), command.AccountId);

            if (!_brokerSyncFactory.SupportedBrokers.Contains(account.Broker.Name))
                return Result<SyncBrokerDto>.Failure(
                        new Error("UnsupportedBroker",
                            $"'{account.Broker.Name}' is not supported. " +
                            $"Supported brokers: {string.Join(", ", _brokerSyncFactory.SupportedBrokers)}"),
                        HttpStatusCode.BadRequest);

            //get broker's credentials 
            var credentials = await _brokerAccountCredentialService.GetAsync<BrokerCredentials>(account.Id, cancellationToken);

            if (credentials is null)
                return Result<SyncBrokerDto>.Failure(
                        new Error("MissingCredentials",
                            $"No credentials found for account '{account.Name}'"),
                        HttpStatusCode.BadRequest);

            //TODO: Add a command property to define data range - All Trades = 365 days back and Last Trades = only from last sync date. 

            DateOnly fromDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-100));
            DateOnly toDate = DateOnly.FromDateTime(DateTime.Today);

            //get the broker sync service from the factory
            var syncService = _brokerSyncFactory.GetSyncService(account.Broker.Name);

            var result = await syncService.SyncAsync(new BrokerSyncRequest(credentials, fromDate, toDate), cancellationToken);

            if (!result.IsSuccess)
            {
                _logger.LogError("Broker sync failed for account {AccountId}: {Code} {Message}", command.AccountId, result.ErrorCode, result.ErrorMessage);
                return Result<SyncBrokerDto>.Failure(new Error("502", result.ErrorMessage ?? "Broker sync failed"), HttpStatusCode.BadGateway);
            }

            // 4 — Check which rows are duplicates
            var brokerExecutionIds = result.Executions
                .Select(r => r.BrokerExecutionId)
                .ToList();

            var existingIds = await _executionRepository
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

            try
            {
                await _unitOfWork.BeginTransactionAsync(cancellationToken);

                await _importSessionRepository.AddAsync(importSession, cancellationToken);

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

                    await _executionRepository.AddAsync(execution, cancellationToken);

                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                //rebuild trades
                var impactedSymbols = orderedExecutions
                      .Select(x => x.Symbol)
                      .Distinct(StringComparer.OrdinalIgnoreCase)
                      .ToArray();

                var newTrades = await _importService.RebuildTradesAsync(command.AccountId, impactedSymbols, cancellationToken);

                await _unitOfWork.CommitAsync(cancellationToken);

                return Result<SyncBrokerDto>.Success(new SyncBrokerDto() { NewTrades = newTrades });

            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}
