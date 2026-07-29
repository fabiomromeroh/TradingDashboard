using AutoMapper;
using System.Net;
using TradingDashboard.Application.Abstractions.Repositories;
using TradingDashboard.Application.Features.ImportSessions.Dtos;
using TradingDashboard.Application.Features.ImportSessions.Queries.GetImportSessionById;
using TradingDashboard.Domain.Entities;
using TradingDashboard.Domain.Enums;

namespace TradingDashboard.UnitTests.Application.ImportSessions.Queries;

public class GetImportSessionByIdQueryHandlerTests
{
    private readonly Mock<IImportSessionRepository> _mockImportSessionRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GetImportSessionByIdQueryHandler _handler;

    public GetImportSessionByIdQueryHandlerTests()
    {
        _mockImportSessionRepository = new Mock<IImportSessionRepository>();
        _mockMapper = new Mock<IMapper>();
        _handler = new GetImportSessionByIdQueryHandler(
            _mockImportSessionRepository.Object,
            _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_WithValidId_ShouldReturnImportSession()
    {
        // Arrange
        var sessionId = Guid.NewGuid();
        var accountId = Guid.NewGuid();
        var fileName = "test_import.csv";
        var createdAt = DateTimeOffset.UtcNow;

        var importSession = ImportSession.Create(accountId, "IBKR", ImportSourceType.FileUpload, fileName);

        var importSessionDto = new ImportSessionDto(
            Id: sessionId,
            FileFormat: "csv",
            FileName: fileName,
            BrokerName: "IBKR",
            Status: "Completed",
            TotalRows: 10,
            SkippedRows: 1,
            ProcessedRows: 9,
            IsRolledBack: false,
            SourceType: "FileUpload",
            CompletedAt: createdAt.AddSeconds(30),
            PeriodStart: DateTimeOffset.Now.AddDays(-10),
            PeriodEnd: DateTimeOffset.Now.AddDays(30),
            AccountId: accountId,
            CreatedAt: createdAt);

        var query = new GetImportSessionByIdQuery(sessionId);

        _mockImportSessionRepository
            .Setup(x => x.GetByIdAsync(sessionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(importSession);

        _mockMapper
            .Setup(x => x.Map<ImportSessionDto>(importSession))
            .Returns(importSessionDto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Id.Should().Be(sessionId);
        result.Value!.FileName.Should().Be(fileName);
        result.Value!.Status.Should().Be("Completed");
        result.Value!.TotalRows.Should().Be(10);
        result.Value!.ProcessedRows.Should().Be(9);
        result.Errors.Should().BeEmpty();

        _mockImportSessionRepository.Verify(
            x => x.GetByIdAsync(sessionId, It.IsAny<CancellationToken>()),
            Times.Once);

        _mockMapper.Verify(
            x => x.Map<ImportSessionDto>(importSession),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonExistentId_ShouldReturnNotFound()
    {
        // Arrange
        var sessionId = Guid.NewGuid();
        var query = new GetImportSessionByIdQuery(sessionId);

        _mockImportSessionRepository
            .Setup(x => x.GetByIdAsync(sessionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ImportSession?)null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().NotBeEmpty();
        result.Errors[0].Code.Should().Be("NotFound");
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.Value.Should().BeNull();

        _mockMapper.Verify(
            x => x.Map<ImportSessionDto>(It.IsAny<ImportSession>()),
            Times.Never);
    }


    [Fact]
    public async Task Handle_WhenRepositoryThrowsException_ShouldPropagateException()
    {
        // Arrange
        var sessionId = Guid.NewGuid();
        var query = new GetImportSessionByIdQuery(sessionId);

        _mockImportSessionRepository
            .Setup(x => x.GetByIdAsync(sessionId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Database connection failed"));

        // Act & Assert
        var act = () => _handler.Handle(query, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Database connection failed");

        _mockMapper.Verify(
            x => x.Map<ImportSessionDto>(It.IsAny<ImportSession>()),
            Times.Never);
    }
}
