using ECommerce.API.Modules.Products.DTOs;

namespace ECommerce.API.Modules.Products.Services;

public interface ICartService
{
    Task<IReadOnlyCollection<ProductInCartResponseDto>> GetAllAsync();
    Task<ProductInCartResponseDto?> GetByIdAsync(int id);
    Task<ProductInCartResponseDto> CreateAsync(ProductInCartRequestDto request);
    Task<ProductInCartResponseDto?> UpdateAsync(int id, ProductInCartRequestDto request);
    Task<bool> DeleteAsync(int id);
}
