using MediatR;
using TradingDashboard.Application.Common.Interfaces;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Application.Features.Trades.Commands.CreateTrade;

public class CreateTradeCommandHandler: IRequestHandler<CreateTradeCommand, Guid>
{
    private readonly ITradeRepository tradeRepository;
    private readonly IUnitOfWork unitOfWork;

    public CreateTradeCommandHandler(ITradeRepository tradeRepository, IUnitOfWork unitOfWork)
    {
        this.tradeRepository = tradeRepository;
        this.unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateTradeCommand command, CancellationToken cancellationToken)
    {

        var trade = Trade.Create(command.Symbol, command.EntryPrice, command.Quantity, command.Direction);

        await tradeRepository.AddTradeAsync(trade, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return trade.Id;
    }
}
