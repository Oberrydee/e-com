using ECommerce.API.Modules.Products.DTOs;

namespace ECommerce.API.Modules.Products.Services;

public interface IProductService
{
    Task<IReadOnlyCollection<ProductResponseDto>> GetAllAsync();
    Task<ProductResponseDto?> GetByIdAsync(int id);
    Task<ProductResponseDto> CreateAsync(ProductRequestDto request);
    Task<ProductResponseDto?> UpdateAsync(int id, ProductRequestDto request);
    Task<bool> DeleteAsync(int id);
}
