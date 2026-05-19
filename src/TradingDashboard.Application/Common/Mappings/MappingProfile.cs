using AutoMapper;
using TradingDashboard.Application.Features.Accounts.Dtos;
using TradingDashboard.Application.Features.ImportSessions.Dtos;
using TradingDashboard.Application.Features.Trades.Dtos;
using TradingDashboard.Application.Features.Users.Dtos;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Trade, TradeDto>();
        CreateMap<User, UserDto>();
        CreateMap<Account, AccountDto>();
        CreateMap<ImportSession, ImportSessionDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
    }
}
