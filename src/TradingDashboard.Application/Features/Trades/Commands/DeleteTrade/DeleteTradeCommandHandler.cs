using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TradingDashboard.Application.Common.Exceptions;
using TradingDashboard.Application.Common.Interfaces;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Application.Features.Trades.Commands.DeleteTrade
{
    public class DeleteTradeCommandHandler: IRequestHandler<DeleteTradeCommand, Unit>
    {
        private readonly ITradeRepository tradeRepository;
        private readonly IUnitOfWork unitOfWork;

        public DeleteTradeCommandHandler(ITradeRepository tradeRepository, IUnitOfWork unitOfWork)
        {
            this.tradeRepository = tradeRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteTradeCommand command, CancellationToken cancellationToken)
        {
            var trade = await tradeRepository.GetTradeAsync(command.Id, cancellationToken) ?? throw new NotFoundException(nameof(Trade), command.Id);

            await tradeRepository.DeleteTradeAsync(trade, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;

        }
    }
}
