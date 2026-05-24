using AutoMapper;
using MediatR;
using TradingDashboard.Application.Common;
using TradingDashboard.Application.Common.Interfaces;
using TradingDashboard.Application.Features.Users.Dtos;

namespace TradingDashboard.Application.Features.Users.Queries.GetUsers
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, Result<IEnumerable<UserDto>>>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public GetUsersQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        public async Task<Result<IEnumerable<UserDto>>> Handle(GetUsersQuery usersQuery, CancellationToken cancellationToken)
        {
            var users = await userRepository.GetAllAsync(cancellationToken);


            return Result<IEnumerable<UserDto>>.Success(mapper.Map<IEnumerable<UserDto>>(users));

        }
    }
}
