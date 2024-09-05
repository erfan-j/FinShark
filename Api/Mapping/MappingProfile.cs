using Api.Dtos.Stocks;
using Api.Models;
using AutoMapper;

namespace Api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Stock, StockDto>(MemberList.Destination);
            CreateMap<Stock, StockDto>(MemberList.Destination).ReverseMap();
            CreateMap<Stock, CreateStockDto>(MemberList.Destination);
            CreateMap<Stock, CreateStockDto>(MemberList.Destination).ReverseMap();
            CreateMap<Stock, UpdateStockDto>(MemberList.Destination);
            CreateMap<Stock, UpdateStockDto>(MemberList.Destination).ReverseMap();
        }
    }
}
