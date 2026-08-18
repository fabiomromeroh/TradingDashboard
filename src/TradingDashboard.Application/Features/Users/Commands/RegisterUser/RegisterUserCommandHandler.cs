using AutoMapper;
using MediatR;
using TradingDashboard.Application.Abstractions;
using TradingDashboard.Application.Abstractions.Repositories;
using TradingDashboard.Application.Common.Models;
using TradingDashboard.Application.Features.Users.Dtos;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Application.Features.Users.Commands.RegisterUser;

public class RegisterUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<RegisterUserCommand, Result<UserDto>>
{
    public async Task<Result<UserDto>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var existing = await userRepository.GetByEmailAsync(command.Email, cancellationToken);
        if (existing is not null)
            return Result<UserDto>.Conflict("A user with this email address already exists.");

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(command.Password);
        var user = User.Create(command.Email, passwordHash, command.FirstName, command.LastName);

        await userRepository.AddAsync(user, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<UserDto>.Success(mapper.Map<UserDto>(user));
    }
}
