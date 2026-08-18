using MediatR;
using TradingDashboard.Application.Abstractions;
using TradingDashboard.Application.Abstractions.Repositories;
using TradingDashboard.Application.Common.Helpers;
using TradingDashboard.Application.Common.Models;

namespace TradingDashboard.Application.Features.Users.Commands.LogoutUser
{
    public class LogoutCommandHandler(IRefreshTokenRepository refreshTokenRepository, IUnitOfWork unitOfWork) : IRequestHandler<LogoutCommand, Result>
    {
        public async Task<Result> Handle(LogoutCommand command, CancellationToken ct)
        {
            var token = await refreshTokenRepository.GetByHashAsync(TokenHasher.Hash(command.RefreshToken), ct);

            if (token is null || token.RevokedAt is not null)
                return Result.Success(); //idempotent

            token.Revoke();

            await refreshTokenRepository.UpdateAsync(token, ct);


            await unitOfWork.SaveChangesAsync(ct);

            return Result.Success();
        }
    }
}
