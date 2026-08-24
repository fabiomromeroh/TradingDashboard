using MediatR;
using System.Text.Json;
using TradingDashboard.Application.Common.Models;

namespace TradingDashboard.Application.Features.Config.Commands.UpdateUserConfigCommand
{
    /// <summary>
    /// Command to update a specific user configuration.
    /// The ConfigType discriminator determines which configuration property is updated.
    /// </summary>
    public record UpdateUserConfigCommand(
        Guid UserId,
        string ConfigType,
        JsonElement Config) : IRequest<Result<Unit>>
    {
    }
}
