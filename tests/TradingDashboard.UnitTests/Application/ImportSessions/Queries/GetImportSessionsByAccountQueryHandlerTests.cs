using AutoMapper;
using TradingDashboard.Application.Common.Interfaces;
using TradingDashboard.Application.Features.ImportSessions.Dtos;
using TradingDashboard.Application.Features.ImportSessions.Queries.GetImportSessionsByAccount;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.UnitTests.Application.ImportSessions.Queries;

public class GetImportSessionsByAccountQueryHandlerTests
{
    private readonly Mock<IImportSessionRepository> _mockImportSessionRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GetImportSessionsByAccountQueryHandler _handler;

    public GetImportSessionsByAccountQueryHandlerTests()
    {
        _mockImportSessionRepository = new Mock<IImportSessionRepository>();
        _mockMapper = new Mock<IMapper>();
        _handler = new GetImportSessionsByAccountQueryHandler(
            _mockImportSessionRepository.Object,
            _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_WithValidAccountId_ShouldReturnAllSessionsForAccount()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var createdAt = DateTimeOffset.UtcNow;

        var importSessions = new List<ImportSession>
        {
            ImportSession.Create(accountId, "import1.csv"),
            ImportSession.Create(accountId, "import2.csv"),
            ImportSession.Create(accountId, "import3.csv")
        };

        var importSessionDtos = new List<ImportSessionDto>
        {
            new(
                Id: importSessions[0].Id,
                FileName: "import1.csv",
                Status: "Completed",
                TotalTrades: 10,
                ImportedTrades: 10,
                ErrorMessage: null,
                CompletedAt: createdAt.AddSeconds(30),
                AccountId: accountId,
                CreatedAt: createdAt),
            new(
                Id: importSessions[1].Id,
                FileName: "import2.csv",
                Status: "Completed",
                TotalTrades: 5,
                ImportedTrades: 4,
                ErrorMessage: null,
                CompletedAt: createdAt.AddMinutes(1).AddSeconds(30),
                AccountId: accountId,
                CreatedAt: createdAt.AddMinutes(1)),
            new(
                Id: importSessions[2].Id,
                FileName: "import3.csv",
                Status: "Pending",
                TotalTrades: 0,
                ImportedTrades: 0,
                ErrorMessage: null,
                CompletedAt: null,
                AccountId: accountId,
                CreatedAt: createdAt.AddMinutes(2))
        };

        var query = new GetImportSessionsByAccountQuery(accountId);

        _mockImportSessionRepository
            .Setup(x => x.GetAllByAccountIdAsync(accountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(importSessions);

        _mockMapper
            .Setup(x => x.Map<IEnumerable<ImportSessionDto>>(importSessions))
            .Returns(importSessionDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Should().HaveCount(3);
        result.Value!.Should().AllSatisfy(s => s.AccountId.Should().Be(accountId));
        result.Value!.Should().Contain(s => s.Status == "Pending");
        result.Value!.Where(s => s.Status == "Completed").Should().HaveCount(2);
        result.Errors.Should().BeEmpty();

        _mockImportSessionRepository.Verify(
            x => x.GetAllByAccountIdAsync(accountId, It.IsAny<CancellationToken>()),
            Times.Once);

        _mockMapper.Verify(
            x => x.Map<IEnumerable<ImportSessionDto>>(importSessions),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithAccountIdHavingNoSessions_ShouldReturnEmptyList()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var importSessions = new List<ImportSession>();
        var importSessionDtos = new List<ImportSessionDto>();

        var query = new GetImportSessionsByAccountQuery(accountId);

        _mockImportSessionRepository
            .Setup(x => x.GetAllByAccountIdAsync(accountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(importSessions);

        _mockMapper
            .Setup(x => x.Map<IEnumerable<ImportSessionDto>>(importSessions))
            .Returns(importSessionDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Should().BeEmpty();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ShouldReturnOnlySessionsForSpecificAccount()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var otherAccountId = Guid.NewGuid();
        var createdAt = DateTimeOffset.UtcNow;

        // Only return sessions for the queried account
        var importSessions = new List<ImportSession>
        {
            ImportSession.Create(accountId, "import1.csv")
        };

        var importSessionDtos = new List<ImportSessionDto>
        {
            new(
                Id: importSessions[0].Id,
                FileName: "import1.csv",
                Status: "Completed",
                TotalTrades: 10,
                ImportedTrades: 10,
                ErrorMessage: null,
                CompletedAt: createdAt.AddSeconds(30),
                AccountId: accountId,
                CreatedAt: createdAt)
        };

        var query = new GetImportSessionsByAccountQuery(accountId);

        _mockImportSessionRepository
            .Setup(x => x.GetAllByAccountIdAsync(accountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(importSessions);

        _mockMapper
            .Setup(x => x.Map<IEnumerable<ImportSessionDto>>(importSessions))
            .Returns(importSessionDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value!.Should().HaveCount(1);
        result.Value!.All(s => s.AccountId == accountId).Should().BeTrue();

        // Verify repository was called with the correct account ID
        _mockImportSessionRepository.Verify(
            x => x.GetAllByAccountIdAsync(accountId, It.IsAny<CancellationToken>()),
            Times.Once);

        _mockImportSessionRepository.Verify(
            x => x.GetAllByAccountIdAsync(otherAccountId, It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrowsException_ShouldPropagateException()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var query = new GetImportSessionsByAccountQuery(accountId);

        _mockImportSessionRepository
            .Setup(x => x.GetAllByAccountIdAsync(accountId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Database connection failed"));

        // Act & Assert
        var act = () => _handler.Handle(query, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Database connection failed");

        _mockMapper.Verify(
            x => x.Map<IEnumerable<ImportSessionDto>>(It.IsAny<IEnumerable<ImportSession>>()),
            Times.Never);
    }
}
