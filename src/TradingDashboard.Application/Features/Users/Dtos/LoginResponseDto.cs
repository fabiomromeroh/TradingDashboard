namespace TradingDashboard.Application.Features.Users.Dtos;

public record LoginResponseDto(string AccessToken, string RefreshToken, UserDto User);
