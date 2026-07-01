using TradingDashboard.Application.Common.Interfaces;
using TradingDashboard.Application.Features.ImportSessions.Commands.ConfirmImport;
using TradingDashboard.Application.Features.ImportSessions.Dtos;
using TradingDashboard.Domain.Entities;
using TradingDashboard.Domain.Enums;

namespace TradingDashboard.UnitTests.Application.ImportSessions.Commands;

public class ConfirmImportCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IImportSessionRepository> _mockImportSessionRepository;
    private readonly Mock<ITradeRepository> _mockTradeRepository;
    private readonly ConfirmImportCommandHandler _handler;

    public ConfirmImportCommandHandlerTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockImportSessionRepository = new Mock<IImportSessionRepository>();
        _mockTradeRepository = new Mock<ITradeRepository>();
        _handler = new ConfirmImportCommandHandler(
            _mockUnitOfWork.Object,
            _mockImportSessionRepository.Object,
            _mockTradeRepository.Object);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldCreateImportSessionAndReturnId()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var fileName = "test_import.csv";
        var executedAt = DateTimeOffset.UtcNow;

        var rows = new List<PreviewRowDto>
        {
            new(
                RowNumber: 1,
                Symbol: "EURUSD",
                Description: "EUR/USD",
                Side: "Buy",
                Quantity: 1.0m,
                Price: 1.0850m,
                Commission: 10m,
                Exchange: "FOREX",
                OrderType: "Market",
                ExecutedAt: executedAt,
                IsDuplicate: false,
                ParseError: null,
                BrokerExecutionId: "EXEC001",
                BrokerOrderId: "ORDER001",
                BrokerTradeId: "TRADE001"
            ),
            new(
                RowNumber: 2,
                Symbol: "GBPUSD",
                Description: "GBP/USD",
                Side: "Sell",
                Quantity: 2.0m,
                Price: 1.2650m,
                Commission: 20m,
                Exchange: "FOREX",
                OrderType: "Market",
                ExecutedAt: executedAt,
                IsDuplicate: false,
                ParseError: null,
                BrokerExecutionId: "EXEC002",
                BrokerOrderId: "ORDER002",
                BrokerTradeId: "TRADE002"
            )
        };

        var command = new ConfirmImportCommand(
            FileName: fileName,
            AccountId: accountId,
            TotalRows: 2,
            NewRows: 2,
            DuplicateRows: 0,
            InvalidRows: 0,
            Rows: rows);

        _mockTradeRepository
            .Setup(x => x.GetOpenTradesByAccountIdAsync(accountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        _mockImportSessionRepository
            .Setup(x => x.AddAsync(It.IsAny<ImportSession>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBe(Guid.Empty);
        result.Errors.Should().BeEmpty();

        _mockTradeRepository.Verify(
            x => x.GetOpenTradesByAccountIdAsync(accountId, It.IsAny<CancellationToken>()),
            Times.Once);

        _mockImportSessionRepository.Verify(
            x => x.AddAsync(It.IsAny<ImportSession>(), It.IsAny<CancellationToken>()),
            Times.Once);

        _mockUnitOfWork.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithDuplicateRows_ShouldOnlyImportNewRows()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var fileName = "test_import.csv";
        var executedAt = DateTimeOffset.UtcNow;

        var rows = new List<PreviewRowDto>
        {
            new(
                RowNumber: 1,
                Symbol: "EURUSD",
                Description: "EUR/USD",
                Side: "Buy",
                Quantity: 1.0m,
                Price: 1.0850m,
                Commission: 10m,
                Exchange: "FOREX",
                OrderType: "Market",
                ExecutedAt: executedAt,
                IsDuplicate: false,
                ParseError: null,
                BrokerExecutionId: "EXEC001",
                BrokerOrderId: "ORDER001",
                BrokerTradeId: "TRADE001"
            ),
            new(
                RowNumber: 2,
                Symbol: "GBPUSD",
                Description: "GBP/USD",
                Side: "Sell",
                Quantity: 2.0m,
                Price: 1.2650m,
                Commission: 20m,
                Exchange: "FOREX",
                OrderType: "Market",
                ExecutedAt: executedAt,
                IsDuplicate: true,
                ParseError: null,
                BrokerExecutionId: "EXEC002",
                BrokerOrderId: "ORDER002",
                BrokerTradeId: "TRADE002"
            )
        };

        var command = new ConfirmImportCommand(
            FileName: fileName,
            AccountId: accountId,
            TotalRows: 2,
            NewRows: 1,
            DuplicateRows: 1,
            InvalidRows: 0,
            Rows: rows);

        _mockTradeRepository
            .Setup(x => x.GetOpenTradesByAccountIdAsync(accountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        _mockImportSessionRepository
            .Setup(x => x.AddAsync(It.IsAny<ImportSession>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBe(Guid.Empty);

        // Verify that only one trade was processed (the non-duplicate one)
        _mockTradeRepository.Verify(
            x => x.AddTradeAsync(It.IsAny<Trade>(), It.IsAny<CancellationToken>()),
            Times.Once); // Only one new trade should be added
    }

    [Fact]
    public async Task Handle_WithExistingOpenTrade_ShouldUpdateTradeWithExecution()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var tradeId = Guid.NewGuid();
        var fileName = "test_import.csv";
        var executedAt = DateTimeOffset.UtcNow;

        var existingTrade = Trade.Create(
            "EURUSD",
            1.0800m,
            1.0m,
            TradeDirection.Long,
            accountId,
            executedAt.AddMinutes(-10));

        var rows = new List<PreviewRowDto>
        {
            new(
                RowNumber: 1,
                Symbol: "EURUSD",
                Description: "EUR/USD",
                Side: "Buy",
                Quantity: 1.0m,
                Price: 1.0850m,
                Commission: 10m,
                Exchange: "FOREX",
                OrderType: "Market",
                ExecutedAt: executedAt,
                IsDuplicate: false,
                ParseError: null,
                BrokerExecutionId: "EXEC001",
                BrokerOrderId: "ORDER001",
                BrokerTradeId: "TRADE001"
            )
        };

        var command = new ConfirmImportCommand(
            FileName: fileName,
            AccountId: accountId,
            TotalRows: 1,
            NewRows: 0,
            DuplicateRows: 0,
            InvalidRows: 0,
            Rows: rows);

        _mockTradeRepository
            .Setup(x => x.GetOpenTradesByAccountIdAsync(accountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync([existingTrade]);

        _mockImportSessionRepository
            .Setup(x => x.AddAsync(It.IsAny<ImportSession>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBe(Guid.Empty);

        // Verify that no new trade was added (only existing trade was updated)
        _mockTradeRepository.Verify(
            x => x.AddTradeAsync(It.IsAny<Trade>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrowsException_ShouldPropagateException()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var fileName = "test_import.csv";
        var executedAt = DateTimeOffset.UtcNow;

        var rows = new List<PreviewRowDto>
        {
            new(
                RowNumber: 1,
                Symbol: "EURUSD",
                Description: "EUR/USD",
                Side: "Buy",
                Quantity: 1.0m,
                Price: 1.0850m,
                Commission: 10m,
                Exchange: "FOREX",
                OrderType: "Market",
                ExecutedAt: executedAt,
                IsDuplicate: false,
                ParseError: null,
                BrokerExecutionId: "EXEC001",
                BrokerOrderId: "ORDER001",
                BrokerTradeId: "TRADE001"
            )
        };

        var command = new ConfirmImportCommand(
            FileName: fileName,
            AccountId: accountId,
            TotalRows: 1,
            NewRows: 1,
            DuplicateRows: 0,
            InvalidRows: 0,
            Rows: rows);

        _mockTradeRepository
            .Setup(x => x.GetOpenTradesByAccountIdAsync(accountId, It.IsAny<CancellationToken>()))
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
