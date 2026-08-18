using TradingDashboard.Application.Abstractions;
using TradingDashboard.Application.Abstractions.Repositories;
using TradingDashboard.Application.Features.Trades.Commands.DeleteTrade;
using TradingDashboard.Domain.Entities;
using TradingDashboard.Domain.Enums;

namespace TradingDashboard.UnitTests.Application.Trades;

public class DeleteTradeCommandHandlerTests
{
    private readonly Mock<ITradeRepository> _mockTradeRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly DeleteTradeCommandHandler _handler;

    public DeleteTradeCommandHandlerTests()
    {
        _mockTradeRepository = new Mock<ITradeRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _handler = new DeleteTradeCommandHandler(_mockTradeRepository.Object, _mockUnitOfWork.Object);
    }

    [Fact]
    public async Task Handle_WithValidTradeId_ShouldDeleteTradeAndSaveChanges()
    {
        // Arrange
        var tradeId = Guid.NewGuid();
        var trade = Trade.Create("EURUSD", 1.0850m, 1.0m, TradeDirection.Long, Guid.NewGuid(), DateTimeOffset.UtcNow);

        var command = new DeleteTradeCommand { Id = tradeId };

        _mockTradeRepository
            .Setup(x => x.GetTradeAsync(tradeId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(trade);

        _mockTradeRepository
            .Setup(x => x.DeleteTradeAsync(trade, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _mockTradeRepository.Verify(
            x => x.GetTradeAsync(tradeId, It.IsAny<CancellationToken>()),
            Times.Once);
        _mockTradeRepository.Verify(
            x => x.DeleteTradeAsync(trade, It.IsAny<CancellationToken>()),
            Times.Once);
        _mockUnitOfWork.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonExistentTradeId_ShouldReturnNotFound()
    {
        // Arrange
        var tradeId = Guid.NewGuid();
        var command = new DeleteTradeCommand { Id = tradeId };

        _mockTradeRepository
            .Setup(x => x.GetTradeAsync(tradeId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Trade?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        result.Errors.Should().ContainSingle(e => e.Code == "NotFound" && e.Message.Contains(nameof(Trade)));

        _mockTradeRepository.Verify(
            x => x.DeleteTradeAsync(It.IsAny<Trade>(), It.IsAny<CancellationToken>()),
            Times.Never);
        _mockUnitOfWork.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenDeleteFails_ShouldNotSaveChanges()
    {
        // Arrange
        var tradeId = Guid.NewGuid();
        var trade = Trade.Create("GBPUSD", 1.2650m, 0.5m, TradeDirection.Short, Guid.NewGuid(), DateTimeOffset.UtcNow);
        var command = new DeleteTradeCommand { Id = tradeId };

        _mockTradeRepository
            .Setup(x => x.GetTradeAsync(tradeId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(trade);

        _mockTradeRepository
            .Setup(x => x.DeleteTradeAsync(trade, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Delete failed"));

        // Act & Assert
        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Delete failed");

        _mockUnitOfWork.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
