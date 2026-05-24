using MediatR;
using TradingDashboard.Application.Common;
using TradingDashboard.Application.Common.Interfaces;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Application.Features.Trades.Commands.DeleteTrade
{
    public class DeleteTradeCommandHandler : IRequestHandler<DeleteTradeCommand, Result>
    {
        private readonly ITradeRepository tradeRepository;
        private readonly IUnitOfWork unitOfWork;

        public DeleteTradeCommandHandler(ITradeRepository tradeRepository, IUnitOfWork unitOfWork)
        {
            this.tradeRepository = tradeRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteTradeCommand command, CancellationToken cancellationToken)
        {
            var trade = await tradeRepository.GetTradeAsync(command.Id, cancellationToken);
            if (trade is null) return Result.NotFound(nameof(Trade), command.Id);

            await tradeRepository.DeleteTradeAsync(trade, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
