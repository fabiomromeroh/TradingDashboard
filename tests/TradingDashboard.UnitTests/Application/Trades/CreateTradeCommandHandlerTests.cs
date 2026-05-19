using TradingDashboard.Application.Features.Trades.Commands.CreateTrade;
using TradingDashboard.Application.Common.Interfaces;
using TradingDashboard.Domain.Entities;
using TradingDashboard.Domain.Enums;

namespace TradingDashboard.UnitTests.Application.Trades;

public class CreateTradeCommandHandlerTests
{
    private readonly Mock<ITradeRepository> _mockTradeRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly CreateTradeCommandHandler _handler;

    public CreateTradeCommandHandlerTests()
    {
        _mockTradeRepository = new Mock<ITradeRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _handler = new CreateTradeCommandHandler(_mockTradeRepository.Object, _mockUnitOfWork.Object);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldCreateTradeAndReturnId()
    {
        // Arrange
        var command = new CreateTradeCommand
        {
            Symbol = "EURUSD",
            EntryPrice = 1.0850m,
            Quantity = 1.0m,
            Direction = TradeDirection.Long
        };

        _mockTradeRepository
            .Setup(x => x.AddTradeAsync(It.IsAny<Trade>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBe(Guid.Empty);
        _mockTradeRepository.Verify(
            x => x.AddTradeAsync(It.IsAny<Trade>(), It.IsAny<CancellationToken>()),
            Times.Once);
        _mockUnitOfWork.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrowsException_ShouldPropagateException()
    {
        // Arrange
        var command = new CreateTradeCommand
        {
            Symbol = "USDJPY",
            EntryPrice = 149.50m,
            Quantity = 2.0m,
            Direction = TradeDirection.Long
        };

        _mockTradeRepository
            .Setup(x => x.AddTradeAsync(It.IsAny<Trade>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Database connection failed"));

        // Act & Assert
        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Database connection failed");

        _mockUnitOfWork.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
