using TradingDashboard.Application.Common.Interfaces;
using TradingDashboard.Application.Services.Import.Interfaces;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Infrastructure.Services.Import
{
    public class ImportSessionService : IImportService
    {
        private readonly IExecutionRepository executionRepository;
        private readonly ITradeRepository tradeRepository;

        public ImportSessionService(IExecutionRepository executionRepository, ITradeRepository tradeRepository)
        {
            this.executionRepository = executionRepository;
            this.tradeRepository = tradeRepository;
        }
        public async Task RebuildTradesAsync(Guid accountId, string[] symbols, CancellationToken ct)
        {
            //get executions by symbol/account
            var executions = await executionRepository.GetByAccountAndSymbolsAsync(accountId, symbols, ct);

            //re-build all trades trades by symbol/account
            var result = await ReplayIntoTradesAsync(executions);

            //remove trades
            tradeRepository.RemoveTradeRangeByAccountAndSymbol(accountId, symbols);

            //create new trades and attach executions
            await tradeRepository.AddTradeRangeAsync(result.Trades, ct);

            //attach executions to new trades
            foreach (var exec in executions)
            {

                exec.AttachToTrade(result.ExecutionToTradeId[exec.Id]);

            }
        }

        private static async Task<ReplayResult> ReplayIntoTradesAsync(IReadOnlyList<Execution> executions)
        {
            var result = new ReplayResult();

            var groupedExec = executions
                .GroupBy(e => e.Symbol, StringComparer.OrdinalIgnoreCase);

            foreach (var symbolGroup in groupedExec)
            {
                var orderedExec = symbolGroup
                    .OrderBy(e => e.ExecutedAt)
                    .ThenBy(e => e.BrokerExecutionId)
                    .ToList();

                Trade? currentTrade = null;
                decimal runningPosition = 0;

                //Re-build trade for symbol/account
                foreach (var exec in orderedExec)
                {
                    if (currentTrade == null)
                    {
                        currentTrade = Trade.CreatePlaceholder(symbolGroup.Key, exec.AccountId);
                        result.Trades.Add(currentTrade);
                    }

                    currentTrade.AddExecution(exec);
                    result.ExecutionToTradeId[exec.Id] = currentTrade.Id;

                    runningPosition = currentTrade.PositionSize;

                    if (runningPosition == 0)
                    {
                        currentTrade = null;
                    }
                }
            }

            return result;
        }

        public sealed class ReplayResult
        {
            public List<Trade> Trades { get; init; } = [];
            public Dictionary<Guid, Guid> ExecutionToTradeId { get; init; } = [];
        }
    }


}
