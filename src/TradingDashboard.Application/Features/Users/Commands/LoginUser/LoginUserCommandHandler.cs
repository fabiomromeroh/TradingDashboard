using AutoMapper;
using MediatR;
using System.Security.Authentication;
using TradingDashboard.Application.Common.Exceptions;
using TradingDashboard.Application.Common.Interfaces;
using TradingDashboard.Application.Features.Users.Dtos;

namespace TradingDashboard.Application.Features.Users.Commands.LoginUser;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginResponseDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IMapper _mapper;

    public LoginUserCommandHandler(IUserRepository userRepository, IJwtTokenService jwtTokenService, IMapper mapper)
    {
        _userRepository = userRepository;
        _jwtTokenService = jwtTokenService;
        _mapper = mapper;
    }

    public async Task<LoginResponseDto> Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(command.Email, cancellationToken)
            ?? throw new AuthenticationException("Login failed. Invalid username or password.");

        if (!BCrypt.Net.BCrypt.Verify(command.Password, user.PasswordHash))
            throw new AuthenticationException("Login failed. Invalid username or password.");

        var token = _jwtTokenService.GenerateToken(user);
        return new LoginResponseDto(token, _mapper.Map<UserDto>(user));
    }
}
