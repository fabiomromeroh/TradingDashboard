using AutoMapper;
using MediatR;
using TradingDashboard.Application.Common;
using TradingDashboard.Application.Common.Interfaces;
using TradingDashboard.Application.Features.Users.Dtos;

namespace TradingDashboard.Application.Features.Users.Commands.LoginUser;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<LoginResponseDto>>
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

    public async Task<Result<LoginResponseDto>> Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(command.Email, cancellationToken);
        if (user is null) return Result<LoginResponseDto>.Unauthorized("Login failed. Invalid username or password.");

        if (!BCrypt.Net.BCrypt.Verify(command.Password, user.PasswordHash))
            return Result<LoginResponseDto>.Unauthorized("Login failed. Invalid username or password.");

        var token = _jwtTokenService.GenerateToken(user);

        return Result<LoginResponseDto>.Success(new LoginResponseDto(token, _mapper.Map<UserDto>(user)));
    }
}
