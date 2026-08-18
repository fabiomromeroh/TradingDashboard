using MediatR;
using TradingDashboard.Application.Abstractions;
using TradingDashboard.Application.Abstractions.Repositories;
using TradingDashboard.Application.Common.Models;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Application.Features.Trades.Commands.CreateTrade;

public class CreateTradeCommandHandler(ITradeRepository tradeRepository, IUnitOfWork unitOfWork) : IRequestHandler<CreateTradeCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateTradeCommand command, CancellationToken cancellationToken)
    {

        var trade = Trade.Create(command.Symbol, command.EntryPrice, command.Quantity, command.Direction, command.AccountId, DateTimeOffset.UtcNow);

        await tradeRepository.AddTradeAsync(trade, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(trade.Id);
    }
}

