namespace TradingDashboard.Application.Features.Users.Dtos;

public record UserDto(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    bool IsActive,
    DateTimeOffset CreatedAt);
