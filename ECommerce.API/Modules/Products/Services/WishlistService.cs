using ECommerce.API.Data;
using ECommerce.API.Modules.Products.DTOs;
using ECommerce.API.Modules.Products.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Modules.Products.Services;

public class WishlistService : IWishlistService
{
    private readonly ApplicationDbContext _dbContext;

    public WishlistService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<WishlistResponseDto>> GetAllAsync()
    {
        var wishlists = await _dbContext.Wishlists
            .AsNoTracking()
            .OrderByDescending(wishlist => wishlist.CreatedAt)
            .ToListAsync();

        return wishlists.Select(MapToResponse).ToList();
    }

    public async Task<WishlistResponseDto?> GetByIdAsync(int id)
    {
        var wishlist = await _dbContext.Wishlists
            .AsNoTracking()
            .FirstOrDefaultAsync(wishlist => wishlist.Id == id);

        return wishlist is null ? null : MapToResponse(wishlist);
    }

    public async Task<WishlistResponseDto> CreateAsync(WishlistRequestDto request)
    {
        await EnsureUserExistsAsync(request.UserId);

        var now = DateTime.UtcNow;
        var wishlist = new Wishlist
        {
            UserId = request.UserId,
            Label = request.Label.Trim(),
            CreatedAt = now,
            UpdatedAt = now
        };

        _dbContext.Wishlists.Add(wishlist);
        await _dbContext.SaveChangesAsync();

        return MapToResponse(wishlist);
    }

    public async Task<WishlistResponseDto?> UpdateAsync(int id, WishlistRequestDto request)
    {
        var wishlist = await _dbContext.Wishlists.FirstOrDefaultAsync(item => item.Id == id);
        if (wishlist is null)
        {
            return null;
        }

        await EnsureUserExistsAsync(request.UserId);

        wishlist.UserId = request.UserId;
        wishlist.Label = request.Label.Trim();
        wishlist.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return MapToResponse(wishlist);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var wishlist = await _dbContext.Wishlists.FirstOrDefaultAsync(item => item.Id == id);
        if (wishlist is null)
        {
            return false;
        }

        _dbContext.Wishlists.Remove(wishlist);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    private async Task EnsureUserExistsAsync(int userId)
    {
        var userExists = await _dbContext.Users
            .AsNoTracking()
            .AnyAsync(user => user.Id == userId);

        if (!userExists)
        {
            throw new InvalidOperationException("User not found.");
        }
    }

    private static WishlistResponseDto MapToResponse(Wishlist wishlist) => new()
    {
        Id = wishlist.Id,
        UserId = wishlist.UserId,
        Label = wishlist.Label,
        CreatedAt = wishlist.CreatedAt,
        UpdatedAt = wishlist.UpdatedAt
    };
}
