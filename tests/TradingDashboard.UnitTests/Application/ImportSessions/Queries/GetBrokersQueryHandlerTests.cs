using AutoMapper;
using TradingDashboard.Application.Abstractions.Repositories;
using TradingDashboard.Application.Features.ImportSessions.Dtos;
using TradingDashboard.Application.Features.ImportSessions.Queries.GetBrokers;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.UnitTests.Application.ImportSessions.Queries;

public class GetBrokersQueryHandlerTests
{
    private readonly Mock<IBrokerRepository> _mockBrokerRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GetBrokersQueryHandler _handler;

    public GetBrokersQueryHandlerTests()
    {
        _mockBrokerRepository = new Mock<IBrokerRepository>();
        _mockMapper = new Mock<IMapper>();
        _handler = new GetBrokersQueryHandler(
            _mockBrokerRepository.Object,
            _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnAllBrokers()
    {
        // Arrange
        var brokers = new List<Broker>
        {
            Broker.Create("MetaTrader5", "MetaTrader 5"),
            Broker.Create("ThinkorSwim", "Thinkorswim"),
            Broker.Create("Interactive Brokers", "Interactive Brokers")
        };

        var brokerDtos = new List<BrokerDto>
        {
            new() { Id = brokers[0].Id, Name = "MetaTrader5" },
            new() { Id = brokers[1].Id, Name = "ThinkorSwim" },
            new() { Id = brokers[2].Id, Name = "Interactive Brokers" }
        };

        var query = new GetBrokersQuery();

        _mockBrokerRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(brokers);

        _mockMapper
            .Setup(x => x.Map<IEnumerable<BrokerDto>>(brokers))
            .Returns(brokerDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Should().HaveCount(3);
        result.Value!.Should().Contain(b => b.Name == "MetaTrader5");
        result.Value!.Should().Contain(b => b.Name == "ThinkorSwim");
        result.Value!.Should().Contain(b => b.Name == "Interactive Brokers");
        result.Errors.Should().BeEmpty();

        _mockBrokerRepository.Verify(
            x => x.GetAllAsync(It.IsAny<CancellationToken>()),
            Times.Once);

        _mockMapper.Verify(
            x => x.Map<IEnumerable<BrokerDto>>(brokers),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithNoBrokers_ShouldReturnEmptyList()
    {
        // Arrange
        var brokers = new List<Broker>();
        var brokerDtos = new List<BrokerDto>();

        var query = new GetBrokersQuery();

        _mockBrokerRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(brokers);

        _mockMapper
            .Setup(x => x.Map<IEnumerable<BrokerDto>>(brokers))
            .Returns(brokerDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Should().BeEmpty();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrowsException_ShouldPropagateException()
    {
        // Arrange
        var query = new GetBrokersQuery();

        _mockBrokerRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Database connection failed"));

        // Act & Assert
        var act = () => _handler.Handle(query, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Database connection failed");

        _mockMapper.Verify(
            x => x.Map<IEnumerable<BrokerDto>>(It.IsAny<IEnumerable<Broker>>()),
            Times.Never);
    }
}
