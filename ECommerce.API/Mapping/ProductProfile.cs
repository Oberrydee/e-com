using AutoMapper;
using ECommerce.API.Common.Enums;
using ECommerce.API.Modules.Products.DTOs;
using ECommerce.API.Modules.Products.Entities;

namespace ECommerce.API.Mapping;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<ProductRequestDto, Product>()
            .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code.Trim()))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Trim()))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description.Trim()))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Trim()))
            .ForMember(dest => dest.InternalReference, opt => opt.MapFrom(src => src.InternalReference.Trim()))
            .ForMember(dest => dest.InventoryStatus, opt => opt.Ignore())
            .ForMember(dest => dest.File, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        CreateMap<Product, ProductResponseDto>()
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src =>
                src.File == null || src.File.BinaryContent.Length == 0
                    ? string.Empty
                    : Convert.ToBase64String(src.File.BinaryContent)))
            .ForMember(dest => dest.ImageFileName, opt => opt.MapFrom(src => src.File == null ? string.Empty : src.File.OriginalFileName))
            .ForMember(dest => dest.ImageContentType, opt => opt.MapFrom(src => src.File == null ? string.Empty : src.File.ContentType))
            .ForMember(dest => dest.ImageSizeInBytes, opt => opt.MapFrom(src => src.File == null ? 0 : src.File.SizeInBytes))
            .ForMember(dest => dest.InventoryStatus, opt => opt.MapFrom(src => ConvertInventoryStatusToString(src.InventoryStatus)));
    }

    private static string ConvertInventoryStatusToString(InventoryStatus inventoryStatus) => inventoryStatus switch
    {
        InventoryStatus.InStock => "INSTOCK",
        InventoryStatus.LowStock => "LOWSTOCK",
        InventoryStatus.OutOfStock => "OUTOFSTOCK",
        _ => throw new InvalidOperationException("Unsupported inventory status.")
    };
}
