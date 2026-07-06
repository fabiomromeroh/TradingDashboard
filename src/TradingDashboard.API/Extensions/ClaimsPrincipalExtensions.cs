using System.Security.Claims;

namespace TradingDashboard.API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            var userIdClaim = user.Claims.First();
            return Guid.TryParse(userIdClaim.Value, out var userId) ? userId : Guid.Empty;
        }
    }

}
