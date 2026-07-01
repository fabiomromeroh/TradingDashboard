using MediatR;
using TradingDashboard.Application.Common;
using TradingDashboard.Application.Common.Interfaces;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Application.Features.Trades.Commands.CreateTrade;

public class CreateTradeCommandHandler : IRequestHandler<CreateTradeCommand, Result<Guid>>
{
    private readonly ITradeRepository tradeRepository;
    private readonly IUnitOfWork unitOfWork;

    public CreateTradeCommandHandler(ITradeRepository tradeRepository, IUnitOfWork unitOfWork)
    {
        this.tradeRepository = tradeRepository;
        this.unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateTradeCommand command, CancellationToken cancellationToken)
    {

        var trade = Trade.Create(command.Symbol, command.EntryPrice, command.Quantity, command.Direction, command.AccountId, DateTimeOffset.UtcNow);

        await tradeRepository.AddTradeAsync(trade, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(trade.Id);
    }
}

