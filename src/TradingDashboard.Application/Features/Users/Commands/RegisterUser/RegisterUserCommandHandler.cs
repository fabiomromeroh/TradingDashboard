using AutoMapper;
using MediatR;
using TradingDashboard.Application.Common;
using TradingDashboard.Application.Common.Exceptions;
using TradingDashboard.Application.Common.Interfaces;
using TradingDashboard.Application.Features.Users.Dtos;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Application.Features.Users.Commands.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<UserDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RegisterUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<UserDto>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var existing = await _userRepository.GetByEmailAsync(command.Email, cancellationToken);
        if (existing is not null)
            return Result<UserDto>.Conflict("A user with this email address already exists.");

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(command.Password);
        var user = User.Create(command.Email, passwordHash, command.FirstName, command.LastName);

        await _userRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<UserDto>.Success(_mapper.Map<UserDto>(user));
    }
}
