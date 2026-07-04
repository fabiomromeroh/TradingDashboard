using TradingDashboard.Application.Common.Interfaces;
using TradingDashboard.Application.Features.ImportSessions.Commands.ConfirmImport;
using TradingDashboard.Application.Features.ImportSessions.Dtos;
using TradingDashboard.Application.Services.Import.Interfaces;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.UnitTests.Application.ImportSessions.Commands;

public class ConfirmImportCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IImportSessionRepository> _mockImportSessionRepository;
    private readonly Mock<IExecutionRepository> _mockExecutionRepository;
    private readonly Mock<IImportService> _mockImportService;
    private readonly ConfirmImportCommandHandler _handler;

    public ConfirmImportCommandHandlerTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockImportSessionRepository = new Mock<IImportSessionRepository>();
        _mockExecutionRepository = new Mock<IExecutionRepository>();
        _mockImportService = new Mock<IImportService>();
        _handler = new ConfirmImportCommandHandler(
            _mockUnitOfWork.Object,
            _mockImportSessionRepository.Object,
            _mockExecutionRepository.Object,
            _mockImportService.Object);
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
            BrokerName: "IBKR",
            AccountId: accountId,
            TotalRows: 2,
            NewRows: 2,
            DuplicateRows: 0,
            InvalidRows: 0,
            Rows: rows);


        _mockImportSessionRepository
            .Setup(x => x.AddAsync(It.IsAny<ImportSession>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mockExecutionRepository
            .Setup(x => x.AddAsync(It.IsAny<Execution>(), It.IsAny<CancellationToken>()))
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

        _mockExecutionRepository.Verify(
            x => x.AddAsync(It.IsAny<Execution>(), It.IsAny<CancellationToken>()),
            Times.Exactly(rows.Count));

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
            BrokerName: "IBKR",
            AccountId: accountId,
            TotalRows: 2,
            NewRows: 1,
            DuplicateRows: 1,
            InvalidRows: 0,
            Rows: rows);

        _mockExecutionRepository
            .Setup(x => x.AddAsync(It.IsAny<Execution>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

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
        _mockExecutionRepository.Verify(
            x => x.AddAsync(It.IsAny<Execution>(), It.IsAny<CancellationToken>()),
            Times.Once); // Only one new execution should be added
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
            BrokerName: "IBKR",
            AccountId: accountId,
            TotalRows: 1,
            NewRows: 1,
            DuplicateRows: 0,
            InvalidRows: 0,
            Rows: rows);

        _mockExecutionRepository
            .Setup(x => x.AddAsync(It.IsAny<Execution>(), It.IsAny<CancellationToken>()))
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
