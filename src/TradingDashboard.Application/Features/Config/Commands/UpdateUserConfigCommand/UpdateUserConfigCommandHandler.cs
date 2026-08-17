using MediatR;
using System.Text.Json;
using TradingDashboard.Application.Abstractions.Repositories;
using TradingDashboard.Application.Common.Models;
using TradingDashboard.Application.Interfaces;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Application.Features.Config.Commands.UpdateFilterCommand
{

    /// <summary>
    /// Handler for the UpdateFilterCommand, responsible for processing the command to update the filter settings for a user.
    /// </summary>
    public class UpdateUserConfigCommandHandler : IRequestHandler<UpdateUserConfigCommand, Result<Unit>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUserConfigCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<Unit>> Handle(UpdateUserConfigCommand request, CancellationToken cancellationToken)
        {

            var userConfig = await _userRepository.GetUserConfigurationAsync(request.UserId, cancellationToken);

            var filters = JsonSerializer.Serialize(request.Filters);


            if (userConfig is not null)
            {
                userConfig.FiltersJson = filters;

                await _userRepository.UpdateUserConfiguration(userConfig, cancellationToken);
            }
            else
            {
                await _userRepository.CreateUserConfigurationAsync(new UserConfiguration { UserId = request.UserId, FiltersJson = filters }, cancellationToken);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
