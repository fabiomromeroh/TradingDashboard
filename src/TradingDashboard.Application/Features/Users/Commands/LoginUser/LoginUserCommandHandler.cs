using AutoMapper;
using MediatR;
using System.Security.Cryptography;
using TradingDashboard.Application.Abstractions;
using TradingDashboard.Application.Abstractions.Repositories;
using TradingDashboard.Application.Abstractions.Services;
using TradingDashboard.Application.Common.Helpers;
using TradingDashboard.Application.Common.Models;
using TradingDashboard.Application.Features.Users.Dtos;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Application.Features.Users.Commands.LoginUser;

public class LoginUserCommandHandler(IRefreshTokenRepository refreshTokenRepository, IUserRepository userRepository, IJwtTokenService jwtTokenService, IMapper mapper, IUnitOfWork unitOfWork) : IRequestHandler<LoginUserCommand, Result<LoginResponseDto>>
{

    public async Task<Result<LoginResponseDto>> Handle(LoginUserCommand command, CancellationToken ct)
    {
        var user = await userRepository.GetByEmailAsync(command.Email, ct);
        if (user is null) return Result<LoginResponseDto>.Unauthorized("Login failed. Invalid username or password.");

        if (!BCrypt.Net.BCrypt.Verify(command.Password, user.PasswordHash))
            return Result<LoginResponseDto>.Unauthorized("Login failed. Invalid username or password.");

        var accessToken = jwtTokenService.GenerateToken(user);

        var rawRefreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        var refreshToken = RefreshToken.Create(user.Id, TokenHasher.Hash(rawRefreshToken));

        await refreshTokenRepository.AddAsync(refreshToken, ct);

        await unitOfWork.SaveChangesAsync(ct);


        return Result<LoginResponseDto>.Success(new LoginResponseDto(accessToken, rawRefreshToken, mapper.Map<UserDto>(user)));
    }
}
