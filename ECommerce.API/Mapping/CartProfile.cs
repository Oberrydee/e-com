using AutoMapper;
using ECommerce.API.Modules.Products.DTOs;
using ECommerce.API.Modules.Products.Entities;

namespace ECommerce.API.Mapping;

public class CartProfile : Profile
{
    public CartProfile()
    {
        CreateMap<ProductInCartRequestDto, ProductInCart>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Cart, opt => opt.Ignore())
            .ForMember(dest => dest.Product, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        CreateMap<ProductInCart, ProductInCartResponseDto>();
    }
}
