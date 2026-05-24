using MediatR;
using TradingDashboard.Application.Common;
using TradingDashboard.Application.Features.Users.Dtos;

namespace TradingDashboard.Application.Features.Users.Queries.GetUsers
{
    public class GetUsersQuery : IRequest<Result<IEnumerable<UserDto>>>
    {
        //Here I should add the pagination fields
    }
}
