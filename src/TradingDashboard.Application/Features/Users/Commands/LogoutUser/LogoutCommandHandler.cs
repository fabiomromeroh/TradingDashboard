using MediatR;
using TradingDashboard.Application.Common;
using TradingDashboard.Application.Common.Helpers;
using TradingDashboard.Application.Common.Interfaces;

namespace TradingDashboard.Application.Features.Users.Commands.LogoutUser
{
    public class LogoutCommandHandler(IRefreshTokenRepository refreshTokenRepository, IUnitOfWork unitOfWork) : IRequestHandler<LogoutCommand, Result>
    {
        private readonly IRefreshTokenRepository refreshTokenRepository = refreshTokenRepository;
        private readonly IUnitOfWork unitOfWork = unitOfWork;

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
