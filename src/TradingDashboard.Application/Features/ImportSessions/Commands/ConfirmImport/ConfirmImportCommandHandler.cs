using MediatR;
using System.Data;
using TradingDashboard.Application.Common;
using TradingDashboard.Application.Common.Exceptions;
using TradingDashboard.Application.Common.Extensions;
using TradingDashboard.Application.Common.Interfaces;
using TradingDashboard.Domain.Entities;
using TradingDashboard.Domain.Enums;

namespace TradingDashboard.Application.Features.ImportSessions.Commands.ConfirmImport
{
    public class ConfirmImportCommandHandler : IRequestHandler<ConfirmImportCommand, Result<Guid>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IImportSessionRepository importSessionRepository;
        private readonly ITradeRepository tradeRepository;

        public ConfirmImportCommandHandler(IUnitOfWork unitOfWork, IImportSessionRepository importSessionRepository, ITradeRepository tradeRepository)
        {
            this.unitOfWork = unitOfWork;
            this.importSessionRepository = importSessionRepository;
            this.tradeRepository = tradeRepository;
        }
        public async Task<Result<Guid>> Handle(ConfirmImportCommand command, CancellationToken ct)
        {

            var orderedRows = command.Rows
                                .Where(r => !r.IsDuplicate)
                                .OrderBy(x => x.ExecutedAt)
                                .ToList();

            var startPeriod = orderedRows.Min(x => x.ExecutedAt);
            var endPeriod = orderedRows.Max(x => x.ExecutedAt);

            //create Import Session
            var importSession = ImportSession.Create(command.AccountId, command.FileName);

            //Load open positions for this account. 
            var openTrades = await tradeRepository.GetOpenTradesByAccountIdAsync(command.AccountId, ct);
            //Key = Symbol - fast lookup during loop, no DB calls inside loop
            var openTradesBySymbol = openTrades.ToDictionary(x => x.Symbol, StringComparer.OrdinalIgnoreCase);

            //Process and save executions and trades
            foreach (var row in orderedRows)
            {

                if (!openTradesBySymbol.TryGetValue(row.Symbol, out var trade))
                {
                    //not found - create new trade
                    trade = Trade.Create(row.Symbol, row.Price, row.Quantity, row.Side.ToEnum().ToTradeDirection(), command.AccountId, row.ExecutedAt);
                    await tradeRepository.AddTradeAsync(trade, ct);
                    openTradesBySymbol[trade.Symbol] = trade; //register in-memory
                }
                var execution = Execution.Create(trade.Id, row.Symbol, row.Price, row.Quantity, row.Side.ToEnum(), row.ExecutedAt, row.Commission, row.BrokerExecutionId, row.BrokerOrderId, importSession.Id, row.Exchange, row.OrderType);
                trade.AddExecution(execution);

                //if the trade is closed, then remove from in-memory collection
                if (trade.Status == TradeStatus.Closed)
                    openTradesBySymbol.Remove(trade.Symbol);

            }

            var importedCount = command.TotalRows - command.DuplicateRows;
            importSession.Complete(command.TotalRows, importedCount, command.DuplicateRows, startPeriod, endPeriod);

            await importSessionRepository.AddAsync(importSession, ct);
            await unitOfWork.SaveChangesAsync(ct);

            return Result<Guid>.Success(importSession.Id);
        }

    }
}
