using AutoMapper;
using TradingDashboard.Application.Features.Trades.Dtos;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Trade, TradeDto>();
    }
}
