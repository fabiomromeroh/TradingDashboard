using MediatR;
using System.Net;
using System.Text.Json;
using TradingDashboard.Application.Abstractions;
using TradingDashboard.Application.Abstractions.Repositories;
using TradingDashboard.Application.Common.Configurations;
using TradingDashboard.Application.Common.Models;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Application.Features.Config.Commands.UpdateUserConfigCommand
{

    /// <summary>
    /// Handler for the UpdateUserConfigCommand, responsible for processing the command to update a specific user configuration.
    /// Supports polymorphic configuration types identified by ConfigType discriminator.
    /// </summary>
    public class UpdateUserConfigCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateUserConfigCommand, Result<Unit>>
    {

        public async Task<Result<Unit>> Handle(UpdateUserConfigCommand request, CancellationToken cancellationToken)
        {

            var userConfig = await userRepository.GetUserConfigurationAsync(request.UserId, cancellationToken);

            // Determine which property to update based on ConfigType
            if (request.ConfigType.Equals("filters", StringComparison.OrdinalIgnoreCase))
            {
                var filters = JsonSerializer.Deserialize<ConfigFilter>(request.Config, AppJsonOptions.Default);
                var filterJson = JsonSerializer.Serialize(filters, AppJsonOptions.Default);

                if (userConfig is not null)
                {
                    userConfig.FiltersJson = filterJson;
                    await userRepository.UpdateUserConfiguration(userConfig, cancellationToken);
                }
                else
                {
                    await userRepository.CreateUserConfigurationAsync(
                        new UserConfiguration
                        {
                            UserId = request.UserId,
                            FiltersJson = filterJson,
                            WidgetLayoutJson = "[]"
                        },
                        cancellationToken);
                }
            }
            else if (request.ConfigType.Equals("dashboard", StringComparison.OrdinalIgnoreCase))
            {
                var dashboard = JsonSerializer.Deserialize<IEnumerable<ConfigDashboard>>(request.Config, AppJsonOptions.Default);
                var dashboardJson = JsonSerializer.Serialize(dashboard, AppJsonOptions.Default);

                if (userConfig is not null)
                {
                    userConfig.WidgetLayoutJson = dashboardJson;
                    await userRepository.UpdateUserConfiguration(userConfig, cancellationToken);
                }
                else
                {
                    await userRepository.CreateUserConfigurationAsync(
                        new UserConfiguration
                        {
                            UserId = request.UserId,
                            FiltersJson = "{}",
                            WidgetLayoutJson = dashboardJson
                        },
                        cancellationToken);
                }
            }
            else
            {
                var error = new Error("InvalidConfigType", $"Unknown configuration type: {request.ConfigType}");
                return Result<Unit>.Failure(error, HttpStatusCode.BadRequest);
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
