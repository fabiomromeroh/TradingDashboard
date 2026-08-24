using AutoMapper;
using TradingDashboard.Application.Abstractions.Services.Metric;
using TradingDashboard.Application.Abstractions.Services.Metric.Specifications;
using TradingDashboard.Application.Abstractions.Services.Trades;
using TradingDashboard.Application.Abstractions.Services.UserConfig;
using TradingDashboard.Application.Features.Config.Dtos;
using TradingDashboard.Application.Features.Config.Extensions;
using TradingDashboard.Application.Features.Trades.Dtos;
using TradingDashboard.Application.Features.Trades.Queries.GetTradesByAccountId;
using TradingDashboard.Domain.Entities;
using TradingDashboard.Domain.Enums;

public class GetTradesByAccountIdQueryHandlerTests
{
    private readonly Mock<ITradeQueryService> _mockTradeQueryService;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GetTradesByAccountIdQueryHandler _handler;
    private readonly Guid _userId;
    private readonly Mock<IUserConfigQueryService> _userConfigQueryService;

    public GetTradesByAccountIdQueryHandlerTests()
    {
        _mockTradeQueryService = new Mock<ITradeQueryService>();
        _mockMapper = new Mock<IMapper>();
        _userConfigQueryService = new Mock<IUserConfigQueryService>();
        _handler = new GetTradesByAccountIdQueryHandler(_mockTradeQueryService.Object, _mockMapper.Object, _userConfigQueryService.Object);
        _userId = Guid.NewGuid();
    }

    [Fact]
    public async Task Handle_WithExistingTradesByAccountId_ShouldReturnTradesAsDtos()
    {
        // Arrange
        var trades = new List<Trade>
        {
            Trade.Create("EURUSD", 1.0850m, 1.0m, TradeDirection.Long, Guid.NewGuid(), DateTimeOffset.UtcNow),
            Trade.Create("GBPUSD", 1.2650m, 0.5m, TradeDirection.Short, Guid.NewGuid(), DateTimeOffset.UtcNow),
            Trade.Create("USDJPY", 149.50m, 2.0m, TradeDirection.Long, Guid.NewGuid(), DateTimeOffset.UtcNow)
        };

        var dtos = trades.Select(t => new TradeDto(t.Id, t.Symbol, t.EntryPrice, t.ClosePrice, t.Quantity, t.PositionSize,
            t.Direction.ToString(), t.Status.ToString(), t.OpenedAt, t.ClosedAt, t.TotalCommissions, t.AverageEntryPrice, t.AverageClosePrice, t.NetReturn, t.PercentageReturn)).ToList();

        var userConfig = new UserConfigDto
        {
            Configs = new List<IUserConfig>
            {
                new FiltersConfig { Filters = null }
            }
        };

        _userConfigQueryService
            .Setup(x => x.GetUserConfigAsync(_userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(userConfig);

        _mockTradeQueryService
            .Setup(x => x.GetTradesAsync(_userId, It.IsAny<ISpecification<Trade>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(trades);

        _mockMapper
            .Setup(m => m.Map<IEnumerable<TradeDto>>(It.IsAny<IEnumerable<Trade>>()))
            .Returns(dtos);

        var query = new GetTradesByAccountIdQuery(_userId);

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

        _mockTradeQueryService.Verify(
            x => x.GetTradesAsync(_userId, It.IsAny<ISpecification<Trade>>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithNoTrades_ShouldReturnEmptyCollection()
    {
        // Arrange
        var emptyTrades = new List<Trade>();
        var userConfig = new UserConfigDto
        {
            Configs = new List<IUserConfig>
            {
                new FiltersConfig { Filters = null }
            }
        };

        _userConfigQueryService
            .Setup(x => x.GetUserConfigAsync(_userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(userConfig);

        _mockTradeQueryService
            .Setup(x => x.GetTradesAsync(_userId, It.IsAny<ISpecification<Trade>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(emptyTrades);

        _mockMapper
            .Setup(m => m.Map<IEnumerable<TradeDto>>(It.IsAny<IEnumerable<Trade>>()))
            .Returns([]);

        var query = new GetTradesByAccountIdQuery(_userId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();

        _mockTradeQueryService.Verify(
            x => x.GetTradesAsync(_userId, It.IsAny<ISpecification<Trade>>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldMapTradesToDtosWithCorrectProperties()
    {
        // Arrange
        var trade = Trade.Create("EURUSD", 1.0850m, 1.5m, TradeDirection.Long, Guid.NewGuid(), DateTimeOffset.UtcNow);
        var trades = new List<Trade> { trade };
        var expectedDto = new TradeDto(trade.Id, trade.Symbol, trade.EntryPrice, trade.ClosePrice, trade.Quantity, trade.PositionSize,
            trade.Direction.ToString(), trade.Status.ToString(), trade.OpenedAt, trade.ClosedAt, trade.TotalCommissions, trade.AverageEntryPrice, trade.AverageClosePrice, trade.NetReturn, trade.PercentageReturn);

        var userConfig = new UserConfigDto
        {
            Configs = new List<IUserConfig>
            {
                new FiltersConfig { Filters = null }
            }
        };

        _userConfigQueryService
            .Setup(x => x.GetUserConfigAsync(_userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(userConfig);

        _mockTradeQueryService
            .Setup(x => x.GetTradesAsync(_userId, It.IsAny<ISpecification<Trade>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(trades);

        _mockMapper
            .Setup(m => m.Map<IEnumerable<TradeDto>>(It.IsAny<IEnumerable<Trade>>()))
            .Returns([expectedDto]);

        var query = new GetTradesByAccountIdQuery(_userId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        var resultList = result.Value!.ToList();
        resultList.Should().HaveCount(1);

        var dto = resultList.First();
        dto.Symbol.Should().Be("EURUSD");
        dto.EntryPrice.Should().Be(1.0850m);
        dto.Quantity.Should().Be(1.5m);
        dto.Direction.Should().Be("Long");
        dto.Status.Should().Be("Open");
    }
}

