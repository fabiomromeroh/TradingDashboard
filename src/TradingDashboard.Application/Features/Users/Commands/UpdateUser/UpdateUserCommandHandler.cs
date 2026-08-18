using AutoMapper;
using MediatR;
using TradingDashboard.Application.Abstractions;
using TradingDashboard.Application.Abstractions.Repositories;
using TradingDashboard.Application.Common.Models;
using TradingDashboard.Application.Features.Users.Dtos;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Application.Features.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateUserCommand, Result<UserDto>>
{
    public async Task<Result<UserDto>> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(command.Id, cancellationToken);
        if (user is null)
            return Result<UserDto>.NotFound(nameof(User), command.Id);

        user.Update(command.FirstName, command.LastName, command.Email);

        await userRepository.UpdateAsync(user, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<UserDto>.Success(mapper.Map<UserDto>(user));
    }
}
