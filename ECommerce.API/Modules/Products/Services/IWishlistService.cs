using ECommerce.API.Modules.Products.DTOs;

namespace ECommerce.API.Modules.Products.Services;

public interface IWishlistService
{
    Task<IReadOnlyCollection<WishlistResponseDto>> GetAllAsync();
    Task<WishlistResponseDto?> GetByIdAsync(int id);
    Task<WishlistResponseDto> CreateAsync(WishlistRequestDto request);
    Task<WishlistResponseDto?> UpdateAsync(int id, WishlistRequestDto request);
    Task<bool> DeleteAsync(int id);
}
