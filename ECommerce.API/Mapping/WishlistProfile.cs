using AutoMapper;
using ECommerce.API.Modules.Products.DTOs;
using ECommerce.API.Modules.Products.Entities;

namespace ECommerce.API.Mapping;

public class WishlistProfile : Profile
{
    public WishlistProfile()
    {
        CreateMap<WishlistRequestDto, Wishlist>()
            .ForMember(dest => dest.Label, opt => opt.MapFrom(src => src.Label.Trim()))
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.ProductsInWishlist, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        CreateMap<Wishlist, WishlistResponseDto>();
    }
}
