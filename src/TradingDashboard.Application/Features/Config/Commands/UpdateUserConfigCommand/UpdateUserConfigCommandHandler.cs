using MediatR;
using System.Text.Json;
using TradingDashboard.Application.Abstractions;
using TradingDashboard.Application.Abstractions.Repositories;
using TradingDashboard.Application.Common.Models;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Application.Features.Config.Commands.UpdateUserConfigCommand
{

    /// <summary>
    /// Handler for the UpdateUserConfigCommand, responsible for processing the command to update the filter settings for a user.
    /// </summary>
    public class UpdateUserConfigCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateUserConfigCommand, Result<Unit>>
    {

        public async Task<Result<Unit>> Handle(UpdateUserConfigCommand request, CancellationToken cancellationToken)
        {

            var userConfig = await userRepository.GetUserConfigurationAsync(request.UserId, cancellationToken);

            var filters = JsonSerializer.Serialize(request.Filters);


            if (userConfig is not null)
            {
                userConfig.FiltersJson = filters;

                await userRepository.UpdateUserConfiguration(userConfig, cancellationToken);
            }
            else
            {
                await userRepository.CreateUserConfigurationAsync(new UserConfiguration { UserId = request.UserId, FiltersJson = filters }, cancellationToken);
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
