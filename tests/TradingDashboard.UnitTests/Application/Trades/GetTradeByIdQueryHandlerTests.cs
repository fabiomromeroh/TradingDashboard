using AutoMapper;
using System.Net;
using TradingDashboard.Application.Common.Interfaces;
using TradingDashboard.Application.Features.Trades.Dtos;
using TradingDashboard.Application.Features.Trades.Queries.GetTradeById;
using TradingDashboard.Domain.Entities;
using TradingDashboard.Domain.Enums;

namespace TradingDashboard.UnitTests.Application.Trades;

public class GetTradeByIdQueryHandlerTests
{
    private readonly Mock<ITradeRepository> _mockTradeRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GetTradeByIdQueryHandler _handler;

    public GetTradeByIdQueryHandlerTests()
    {
        _mockTradeRepository = new Mock<ITradeRepository>();
        _mockMapper = new Mock<IMapper>();
        _handler = new GetTradeByIdQueryHandler(_mockTradeRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_WithValidId_ShouldReturnTradeDto()
    {
        // Arrange
        var tradeId = Guid.NewGuid();
        var trade = Trade.Create("EURUSD", 1.0850m, 1.0m, TradeDirection.Long, Guid.NewGuid(), DateTimeOffset.UtcNow);
        var expectedDto = new TradeDto(
            tradeId,
            "EURUSD",
            1.0850m,
            1.1m,
            1.0m,
            0m,
            "Long",
            "Open",
            DateTimeOffset.UtcNow,
            DateTimeOffset.UtcNow,
            0.36m,
            1.0850m,
            1.1m,
            10,
            30
        );

        var query = new GetTradeByIdQuery(tradeId);

        _mockTradeRepository
            .Setup(x => x.GetTradeAsync(tradeId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(trade);

        _mockMapper
            .Setup(x => x.Map<TradeDto>(trade))
            .Returns(expectedDto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(expectedDto);

        _mockTradeRepository.Verify(
            x => x.GetTradeAsync(tradeId, It.IsAny<CancellationToken>()),
            Times.Once);
        _mockMapper.Verify(
            x => x.Map<TradeDto>(trade),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonExistentId_ShouldReturnNotFound()
    {
        // Arrange
        var tradeId = Guid.NewGuid();
        var query = new GetTradeByIdQuery(tradeId);

        _mockTradeRepository
            .Setup(x => x.GetTradeAsync(tradeId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Trade?)null);

        // Act & Assert
        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.Errors.Should().ContainSingle(e => e.Code == "NotFound" && e.Message.Contains(nameof(Trade)));

        _mockTradeRepository.Verify(x => x.GetTradeAsync(tradeId, CancellationToken.None), Times.Once);


        _mockMapper.Verify(
            x => x.Map<TradeDto>(It.IsAny<Trade>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WithValidId_ShouldCallRepositoryWithCorrectParameters()
    {
        // Arrange
        var tradeId = Guid.NewGuid();
        var trade = Trade.Create("GBPUSD", 1.2650m, 0.5m, TradeDirection.Short, Guid.NewGuid(), DateTimeOffset.UtcNow);
        var expectedDto = new TradeDto(
            tradeId,
            "GBPUSD",
            1.2650m,
            1.1m,
            0.5m,
            0.5m,
            "Short",
            "Open",
            DateTimeOffset.UtcNow,
            DateTimeOffset.UtcNow,
            0.36m,
            1.0850m,
            1.1m,
            10,
            30
        );

        var query = new GetTradeByIdQuery(tradeId);
        var cancellationToken = CancellationToken.None;

        _mockTradeRepository
            .Setup(x => x.GetTradeAsync(tradeId, cancellationToken))
            .ReturnsAsync(trade);

        _mockMapper
            .Setup(x => x.Map<TradeDto>(trade))
            .Returns(expectedDto);

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        _mockTradeRepository.Verify(
            x => x.GetTradeAsync(tradeId, cancellationToken),
            Times.Once);
    }
}
