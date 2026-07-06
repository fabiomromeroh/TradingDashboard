namespace TradingDashboard.Application.Features.Users.Dtos
{
    public record RefreshTokenDto(string AccessToken, string RefreshToken, UserDto User);

}
