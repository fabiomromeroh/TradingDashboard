using MediatR;
using System.Security.Cryptography;
using TradingDashboard.Application.Common.Interfaces;
using TradingDashboard.Domain.Entities;
using TradingDashboard.Domain.Enums;

namespace TradingDashboard.Application.Features.Trades.Commands.CreateTrade;

public class CreateTradeCommandHandler: IRequestHandler<CreateTradeCommand, Guid>
{
    private readonly ITradeRepository tradeRepository;

    public CreateTradeCommandHandler(ITradeRepository tradeRepository)
    {
        this.tradeRepository = tradeRepository;
    }

    public async Task<Guid> Handle(CreateTradeCommand command, CancellationToken cancellationToken)
    {

        var trade = Trade.Create(command.Symbol, command.EntryPrice, command.Quantity, command.Direction);

        await tradeRepository.AddTradeAsync(trade, cancellationToken);

        return trade.Id;
    }
}
