using AutoMapper;
using MediatR;
using System.Security.Cryptography;
using TradingDashboard.Application.Abstractions.Repositories;
using TradingDashboard.Application.Abstractions.Services;
using TradingDashboard.Application.Common.Helpers;
using TradingDashboard.Application.Common.Models;
using TradingDashboard.Application.Features.Users.Dtos;
using TradingDashboard.Application.Interfaces;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Application.Features.Users.Commands.RefreshTokenUser
{
    public class RefreshTokenCommandHandler(
    IRefreshTokenRepository refreshTokenRepository,
    IUserRepository userRepository,
    IJwtTokenService jwtTokenService,
    IMapper mapper,
    IUnitOfWork unitOfWork) : IRequestHandler<RefreshTokenCommand, Result<RefreshTokenDto>>
    {
        private readonly IRefreshTokenRepository refreshTokenRepository = refreshTokenRepository;
        private readonly IUserRepository userRepository = userRepository;
        private readonly IJwtTokenService jwtTokenService = jwtTokenService;
        private readonly IMapper mapper = mapper;
        private readonly IUnitOfWork unitOfWork = unitOfWork;

        public async Task<Result<RefreshTokenDto>> Handle(RefreshTokenCommand command, CancellationToken ct)
        {
            var tokenHash = TokenHasher.Hash(command.RawRefreshToken);
            var stored = await refreshTokenRepository.GetByHashAsync(tokenHash, ct);

            if (stored is null || stored.RevokedAt is not null || stored.ExpiresAt < DateTime.UtcNow)
                return Result<RefreshTokenDto>.Unauthorized("Session expired. Please log in again.");

            var user = await userRepository.GetByIdAsync(stored.UserId, ct);
            if (user is null)
                return Result<RefreshTokenDto>.Unauthorized("User not found.");

            // Rotate: kill the old token, mint a new one, push the sliding window forward
            stored.Revoke();

            var newRawToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var newRefreshToken = RefreshToken.Create(user.Id, TokenHasher.Hash(newRawToken));
            stored.Replace(newRefreshToken.TokenHash);

            await refreshTokenRepository.AddAsync(newRefreshToken, ct);
            await refreshTokenRepository.UpdateAsync(stored, ct);
            await unitOfWork.SaveChangesAsync(ct);

            var newAccessToken = jwtTokenService.GenerateToken(user);

            return Result<RefreshTokenDto>.Success(new RefreshTokenDto(newAccessToken, newRawToken, mapper.Map<UserDto>(user)));
        }
    }
}
