using TradingDashboard.Application.Common.Models;

namespace TradingDashboard.Application.Features.Config.Dtos
{
    public class UserConfigurationDto
    {
        public required QueryFilter Filters { get; set; }
    }
}
