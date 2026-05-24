using TradingDashboard.Application.Features.Trades.Queries.GetAllTrades;
using TradingDashboard.Application.Common.Interfaces;
using TradingDashboard.Application.Features.Trades.Dtos;
using TradingDashboard.Domain.Entities;
using TradingDashboard.Domain.Enums;
using AutoMapper;

namespace TradingDashboard.UnitTests.Application.Trades;

public class GetAllTradesQueryHandlerTests
{
    private readonly Mock<ITradeRepository> _mockTradeRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GetAllTradesQueryHandler _handler;

    public GetAllTradesQueryHandlerTests()
    {
        _mockTradeRepository = new Mock<ITradeRepository>();
        _mockMapper = new Mock<IMapper>();
        _handler = new GetAllTradesQueryHandler(_mockTradeRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_WithExistingTrades_ShouldReturnAllTradesAsDtos()
    {
        // Arrange
        var trades = new List<Trade>
        {
            Trade.Create("EURUSD", 1.0850m, 1.0m, TradeDirection.Long),
            Trade.Create("GBPUSD", 1.2650m, 0.5m, TradeDirection.Short),
            Trade.Create("USDJPY", 149.50m, 2.0m, TradeDirection.Long)
        };

        var dtos = trades.Select(t => new TradeDto(t.Id, t.Symbol, t.EntryPrice, t.Quantity,
            t.Direction.ToString(), t.Status.ToString(), t.OpenedAt)).ToList();

        _mockTradeRepository
            .Setup(x => x.GetTradesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(trades);

        _mockMapper
            .Setup(m => m.Map<IEnumerable<TradeDto>>(It.IsAny<IEnumerable<Trade>>()))
            .Returns(dtos);

        var query = new GetAllTradesQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(3);
        result.Value.Should().AllSatisfy(dto => dto.Should().NotBeNull());

        var resultList = result.Value!.ToList();
        resultList[0].Symbol.Should().Be("EURUSD");
        resultList[1].Symbol.Should().Be("GBPUSD");
        resultList[2].Symbol.Should().Be("USDJPY");

        _mockTradeRepository.Verify(
            x => x.GetTradesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithNoTrades_ShouldReturnEmptyCollection()
    {
        // Arrange
        var emptyTrades = new List<Trade>();

        _mockTradeRepository
            .Setup(x => x.GetTradesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(emptyTrades);

        _mockMapper
            .Setup(m => m.Map<IEnumerable<TradeDto>>(It.IsAny<IEnumerable<Trade>>()))
            .Returns(Enumerable.Empty<TradeDto>());

        var query = new GetAllTradesQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
        _mockTradeRepository.Verify(
            x => x.GetTradesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldMapTradesToDtosWithCorrectProperties()
    {
        // Arrange
        var trade = Trade.Create("EURUSD", 1.0850m, 1.5m, TradeDirection.Long);
        var trades = new List<Trade> { trade };
        var expectedDto = new TradeDto(trade.Id, trade.Symbol, trade.EntryPrice, trade.Quantity,
            trade.Direction.ToString(), trade.Status.ToString(), trade.OpenedAt);

        _mockTradeRepository
            .Setup(x => x.GetTradesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(trades);

        _mockMapper
            .Setup(m => m.Map<IEnumerable<TradeDto>>(It.IsAny<IEnumerable<Trade>>()))
            .Returns(new List<TradeDto> { expectedDto });

        var query = new GetAllTradesQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        var resultList = ((IEnumerable<TradeDto>)result.Value!).ToList();
        resultList.Should().HaveCount(1);

        var dto = resultList.First();
        dto.Symbol.Should().Be("EURUSD");
        dto.EntryPrice.Should().Be(1.0850m);
        dto.Quantity.Should().Be(1.5m);
        dto.Direction.Should().Be("Long");
        dto.Status.Should().Be("Open");
    }
}
