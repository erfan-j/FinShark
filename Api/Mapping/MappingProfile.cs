using Api.Dtos.Comments;
using Api.Dtos.Stocks;
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
        }
    }
}
