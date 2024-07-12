using AutoMapper;
using TheRaven.MVC.Models;
using TheRaven.Shared.Dto;
using TheRaven.Shared.Entity;

namespace TheRaven.MVC.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Order, OrderDto>().ReverseMap();
        CreateMap<OrderDto, OrderCheckoutModel>().ReverseMap();
        CreateMap<Star, StarDto>().ReverseMap();
        //CreateMap<Favourite, FavouriteDto>().ReverseMap();
        //CreateMap<Discount, DiscountDto>().ReverseMap();
    }
}
