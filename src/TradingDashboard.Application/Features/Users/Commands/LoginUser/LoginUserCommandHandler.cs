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

namespace TradingDashboard.Application.Features.Users.Commands.LoginUser;

public class LoginUserCommandHandler(IRefreshTokenRepository refreshTokenRepository, IUserRepository userRepository, IJwtTokenService jwtTokenService, IMapper mapper, IUnitOfWork unitOfWork) : IRequestHandler<LoginUserCommand, Result<LoginResponseDto>>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IJwtTokenService _jwtTokenService = jwtTokenService;
    private readonly IMapper _mapper = mapper;
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly IRefreshTokenRepository refreshTokenRepository = refreshTokenRepository;


    public async Task<Result<LoginResponseDto>> Handle(LoginUserCommand command, CancellationToken ct)
    {
        var user = await _userRepository.GetByEmailAsync(command.Email, ct);
        if (user is null) return Result<LoginResponseDto>.Unauthorized("Login failed. Invalid username or password.");

        if (!BCrypt.Net.BCrypt.Verify(command.Password, user.PasswordHash))
            return Result<LoginResponseDto>.Unauthorized("Login failed. Invalid username or password.");

        var accessToken = _jwtTokenService.GenerateToken(user);

        var rawRefreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        var refreshToken = RefreshToken.Create(user.Id, TokenHasher.Hash(rawRefreshToken));

        await refreshTokenRepository.AddAsync(refreshToken, ct);

        await unitOfWork.SaveChangesAsync(ct);


        return Result<LoginResponseDto>.Success(new LoginResponseDto(accessToken, rawRefreshToken, _mapper.Map<UserDto>(user)));
    }
}
