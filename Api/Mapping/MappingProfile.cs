using Api.Dtos.Comments;
using Api.Dtos.Stocks;
using Api.Dtos.Users;
using Api.Models;
using AutoMapper;

namespace Api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Stock:
            CreateMap<Stock, StockDto>(MemberList.Destination);
            CreateMap<Stock, StockDto>(MemberList.Destination).ReverseMap();
            CreateMap<Stock, CreateStockDto>(MemberList.Destination);
            CreateMap<Stock, CreateStockDto>(MemberList.Destination).ReverseMap();
            CreateMap<Stock, UpdateStockDto>(MemberList.Destination);
            CreateMap<Stock, UpdateStockDto>(MemberList.Destination).ReverseMap();

            //Comment:
            CreateMap<Comment, CommentDto>(MemberList.Destination);
            CreateMap<Comment, CommentDto>(MemberList.Destination).ReverseMap();
            CreateMap<Comment, CreateCommentDto>(MemberList.Destination);
            CreateMap<Comment, CreateCommentDto>(MemberList.Destination).ReverseMap();
            CreateMap<Comment, UpdateCommentDto>(MemberList.Destination);
            CreateMap<Comment, UpdateCommentDto>(MemberList.Destination).ReverseMap();

            //Portfolio
            CreateMap<Portfolio, Stock>()
           .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.StockId))
           .ForMember(dest => dest.Symbol, opt => opt.MapFrom(src => src.Stock!.Symbol))
           .ForMember(dest => dest.Purchase, opt => opt.MapFrom(src => src.Stock!.Purchase))
           .ForMember(dest => dest.LastDiv, opt => opt.MapFrom(src => src.Stock!.LastDiv))
           .ForMember(dest => dest.Industry, opt => opt.MapFrom(src => src.Stock!.Industry))
           .ForMember(dest => dest.MarketCap, opt => opt.MapFrom(src => src.Stock!.MarketCap));

            //User:
            CreateMap<User, UserDto>(MemberList.Destination);
            CreateMap<User, UserDto>(MemberList.Destination).ReverseMap();
        }
    }
}
