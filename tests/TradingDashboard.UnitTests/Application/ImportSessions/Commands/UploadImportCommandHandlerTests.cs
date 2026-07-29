using System.Net;
using TradingDashboard.Application.Abstractions.Repositories;
using TradingDashboard.Application.Abstractions.Services.FileUpload.Models;
using TradingDashboard.Application.Abstractions.Services.Import;
using TradingDashboard.Application.Abstractions.Services.Import.Models;
using TradingDashboard.Application.Features.ImportSessions.Commands.UploadImport;

namespace TradingDashboard.UnitTests.Application.ImportSessions.Commands;

public class UploadImportCommandHandlerTests
{
    private readonly Mock<IBrokerParserFactory> _mockBrokerParserFactory;
    private readonly Mock<IExecutionRepository> _mockExecutionRepository;
    private readonly Mock<IBrokerParser> _mockBrokerParser;
    private readonly UploadImportCommandHandler _handler;
    private static readonly string[] sourceArray = ["MetaTrader5", "ThinkorSwim"];

    public UploadImportCommandHandlerTests()
    {
        _mockBrokerParserFactory = new Mock<IBrokerParserFactory>();
        _mockExecutionRepository = new Mock<IExecutionRepository>();
        _mockBrokerParser = new Mock<IBrokerParser>();
        _handler = new UploadImportCommandHandler(
            _mockBrokerParserFactory.Object,
            _mockExecutionRepository.Object);
    }

    [Fact]
    public async Task Handle_WithValidFile_ShouldReturnPreviewWithParsedRows()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var fileName = "test_import.csv";
        var brokerName = "MetaTrader5";
        var fileContent = new byte[] { 1, 2, 3, 4, 5 };
        var executedAt = DateTimeOffset.UtcNow;

        var command = new UploadImportCommand(
            FileContent: fileContent,
            FileName: fileName,
            BrokerName: brokerName,
            AccountId: accountId);

        var parsedRows = new List<RawExecutionRow>
        {
            new(
                RowNumber: 1,
                BrokerExecutionId: "EXEC001",
                BrokerOrderId: "ORDER001",
                BrokerTradeId: "TRADE001",
                Symbol: "EURUSD",
                Description: "EUR/USD",
                AssetClass: "FX",
                Currency: "USD",
                Side: "Buy",
                Quantity: 1.0m,
                Price: 1.0850m,
                Commission: 10m,
                Exchange: "FOREX",
                OrderType: "Market",
                ExecutedAt: executedAt
            )
        };

        var parseResult = new ParsedImportResult(
            Rows: parsedRows,
            ParseErrors: []
        );

        _mockBrokerParserFactory
            .Setup(x => x.SupportedBrokers)
            .Returns([brokerName]);

        _mockBrokerParserFactory
            .Setup(x => x.GetParser(brokerName))
            .Returns(_mockBrokerParser.Object);

        _mockBrokerParser
            .Setup(x => x.Parse(fileContent))
            .Returns(parseResult);

        _mockExecutionRepository
            .Setup(x => x.GetExistingBrokerExecutionIdsAsync(
                It.IsAny<List<string>>(),
                accountId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.FileName.Should().Be(fileName);
        result.Value!.BrokerName.Should().Be(brokerName);
        result.Value!.AccountId.Should().Be(accountId);
        result.Value!.TotalRows.Should().Be(1);
        result.Value!.NewRows.Should().Be(1);
        result.Value!.DuplicateRows.Should().Be(0);
        result.Value!.InvalidRows.Should().Be(0);
        result.Value!.Rows.Should().HaveCount(1);

        _mockBrokerParserFactory.Verify(x => x.GetParser(brokerName), Times.Once);
        _mockBrokerParser.Verify(x => x.Parse(fileContent), Times.Once);
    }

    [Fact]
    public async Task Handle_WithUnsupportedBroker_ShouldReturnFailure()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var fileName = "test_import.csv";
        var unsupportedBroker = "UnsupportedBroker";
        var fileContent = new byte[] { 1, 2, 3, 4, 5 };

        var command = new UploadImportCommand(
            FileContent: fileContent,
            FileName: fileName,
            BrokerName: unsupportedBroker,
            AccountId: accountId);

        _mockBrokerParserFactory
            .Setup(x => x.SupportedBrokers)
            .Returns(sourceArray.ToList());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().NotBeEmpty();
        result.Errors[0].Code.Should().Be("UnsupportedBroker");
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        _mockBrokerParserFactory.Verify(x => x.GetParser(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WithInvalidFileFormat_ShouldReturnParseErrors()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var fileName = "test_import.csv";
        var brokerName = "MetaTrader5";
        var fileContent = new byte[] { 1, 2, 3, 4, 5 };

        var command = new UploadImportCommand(
            FileContent: fileContent,
            FileName: fileName,
            BrokerName: brokerName,
            AccountId: accountId);

        var parseResult = new ParsedImportResult(
            Rows: [],
            ParseErrors: ["Invalid column header", "Missing required field: Symbol"]
        );

        _mockBrokerParserFactory
            .Setup(x => x.SupportedBrokers)
            .Returns([brokerName]);

        _mockBrokerParserFactory
            .Setup(x => x.GetParser(brokerName))
            .Returns(_mockBrokerParser.Object);

        _mockBrokerParser
            .Setup(x => x.Parse(fileContent))
            .Returns(parseResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
        result.Errors[0].Code.Should().Be("InvalidFormat");
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Handle_WithEmptyFile_ShouldReturnError()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var fileName = "test_import.csv";
        var brokerName = "MetaTrader5";
        var fileContent = new byte[] { 1, 2, 3, 4, 5 };

        var command = new UploadImportCommand(
            FileContent: fileContent,
            FileName: fileName,
            BrokerName: brokerName,
            AccountId: accountId);

        var parseResult = new ParsedImportResult(
            Rows: [],
            ParseErrors: []
        );

        _mockBrokerParserFactory
            .Setup(x => x.SupportedBrokers)
            .Returns(new[] { brokerName }.ToList());

        _mockBrokerParserFactory
            .Setup(x => x.GetParser(brokerName))
            .Returns(_mockBrokerParser.Object);

        _mockBrokerParser
            .Setup(x => x.Parse(fileContent))
            .Returns(parseResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
        result.Errors[0].Code.Should().Be("EmptyFile");
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Handle_WithDuplicateExecutions_ShouldMarkRowsAsDuplicates()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var fileName = "test_import.csv";
        var brokerName = "MetaTrader5";
        var fileContent = new byte[] { 1, 2, 3, 4, 5 };
        var executedAt = DateTimeOffset.UtcNow;

        var command = new UploadImportCommand(
            FileContent: fileContent,
            FileName: fileName,
            BrokerName: brokerName,
            AccountId: accountId);

        var parsedRows = new List<RawExecutionRow>
        {
            new(
                RowNumber: 1,
                BrokerExecutionId: "EXEC001",
                BrokerOrderId: "ORDER001",
                BrokerTradeId: "TRADE001",
                Symbol: "EURUSD",
                Description: "EUR/USD",
                AssetClass: "FX",
                Currency: "USD",
                Side: "Buy",
                Quantity: 1.0m,
                Price: 1.0850m,
                Commission: 10m,
                Exchange: "FOREX",
                OrderType: "Market",
                ExecutedAt: executedAt
            ),
            new(
                RowNumber: 2,
                BrokerExecutionId: "EXEC002",
                BrokerOrderId: "ORDER002",
                BrokerTradeId: "TRADE002",
                Symbol: "GBPUSD",
                Description: "GBP/USD",
                AssetClass: "FX",
                Currency: "USD",
                Side: "Sell",
                Quantity: 2.0m,
                Price: 1.2650m,
                Commission: 20m,
                Exchange: "FOREX",
                OrderType: "Market",
                ExecutedAt: executedAt
            )
        };

        var parseResult = new ParsedImportResult(
            Rows: parsedRows,
            ParseErrors: []
        );

        _mockBrokerParserFactory
            .Setup(x => x.SupportedBrokers)
            .Returns([brokerName]);

        _mockBrokerParserFactory
            .Setup(x => x.GetParser(brokerName))
            .Returns(_mockBrokerParser.Object);

        _mockBrokerParser
            .Setup(x => x.Parse(fileContent))
            .Returns(parseResult);

        // Simulate that EXEC001 already exists in the database
        _mockExecutionRepository
            .Setup(x => x.GetExistingBrokerExecutionIdsAsync(
                It.IsAny<List<string>>(),
                accountId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(["EXEC001"]);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value!.TotalRows.Should().Be(2);
        result.Value!.NewRows.Should().Be(1);
        result.Value!.DuplicateRows.Should().Be(1);

        var previewRows = result.Value!.Rows.ToList();
        previewRows.Should().HaveCount(2);
        previewRows[0].IsDuplicate.Should().BeTrue();
        previewRows[1].IsDuplicate.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrowsException_ShouldPropagateException()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var fileName = "test_import.csv";
        var brokerName = "MetaTrader5";
        var fileContent = new byte[] { 1, 2, 3, 4, 5 };

        var command = new UploadImportCommand(
            FileContent: fileContent,
            FileName: fileName,
            BrokerName: brokerName,
            AccountId: accountId);

        var parsedRows = new List<RawExecutionRow>
        {
            new(
                RowNumber: 1,
                BrokerExecutionId: "EXEC001",
                BrokerOrderId: "ORDER001",
                BrokerTradeId: "TRADE001",
                Symbol: "EURUSD",
                Description: "EUR/USD",
                AssetClass: "FX",
                Currency: "USD",
                Side: "Buy",
                Quantity: 1.0m,
                Price: 1.0850m,
                Commission: 10m,
                Exchange: "FOREX",
                OrderType: "Market",
                ExecutedAt: DateTimeOffset.UtcNow
            )
        };

        var parseResult = new ParsedImportResult(
            Rows: parsedRows,
            ParseErrors: []
        );

        _mockBrokerParserFactory
            .Setup(x => x.SupportedBrokers)
            .Returns([brokerName]);

        _mockBrokerParserFactory
            .Setup(x => x.GetParser(brokerName))
            .Returns(_mockBrokerParser.Object);

        _mockBrokerParser
            .Setup(x => x.Parse(fileContent))
            .Returns(parseResult);

        _mockExecutionRepository
            .Setup(x => x.GetExistingBrokerExecutionIdsAsync(
                It.IsAny<List<string>>(),
                accountId,
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Database connection failed"));

        // Act & Assert
        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Database connection failed");
    }
}
