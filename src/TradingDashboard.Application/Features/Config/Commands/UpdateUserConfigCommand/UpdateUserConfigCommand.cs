using MediatR;
using System.Text.Json;
using TradingDashboard.Application.Common.Models;

namespace TradingDashboard.Application.Features.Config.Commands.UpdateFilterCommand
{
    public record UpdateUserConfigCommand(Guid UserId, JsonElement Filters) : IRequest<Result<Unit>>
    {
    }
}
