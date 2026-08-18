using MediatR;
using TradingDashboard.Application.Abstractions;
using TradingDashboard.Application.Abstractions.Repositories;
using TradingDashboard.Application.Common.Models;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Application.Features.Trades.Commands.DeleteTrade
{
    public class DeleteTradeCommandHandler(ITradeRepository tradeRepository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteTradeCommand, Result>
    {
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
