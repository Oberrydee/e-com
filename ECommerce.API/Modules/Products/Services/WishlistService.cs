using AutoMapper;
using ECommerce.API.Data;
using ECommerce.API.Modules.Products.DTOs;
using ECommerce.API.Modules.Products.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Modules.Products.Services;

public class WishlistService : IWishlistService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public WishlistService(ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<IReadOnlyCollection<WishlistResponseDto>> GetAllAsync()
    {
        var wishlists = await _dbContext.Wishlists
            .AsNoTracking()
            .OrderByDescending(wishlist => wishlist.CreatedAt)
            .ToListAsync();

        return _mapper.Map<IReadOnlyCollection<WishlistResponseDto>>(wishlists);
    }

    public async Task<WishlistResponseDto?> GetByIdAsync(int id)
    {
        var wishlist = await _dbContext.Wishlists
            .AsNoTracking()
            .FirstOrDefaultAsync(wishlist => wishlist.Id == id);

        return wishlist is null ? null : _mapper.Map<WishlistResponseDto>(wishlist);
    }

    public async Task<WishlistResponseDto> CreateAsync(WishlistRequestDto request)
    {
        await EnsureUserExistsAsync(request.UserId);

        var now = DateTime.UtcNow;
        var wishlist = _mapper.Map<Wishlist>(request);
        wishlist.CreatedAt = now;
        wishlist.UpdatedAt = now;

        _dbContext.Wishlists.Add(wishlist);
        await _dbContext.SaveChangesAsync();

        return _mapper.Map<WishlistResponseDto>(wishlist);
    }

    public async Task<WishlistResponseDto?> UpdateAsync(int id, WishlistRequestDto request)
    {
        var wishlist = await _dbContext.Wishlists.FirstOrDefaultAsync(item => item.Id == id);
        if (wishlist is null)
        {
            return null;
        }

        await EnsureUserExistsAsync(request.UserId);

        _mapper.Map(request, wishlist);
        wishlist.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return _mapper.Map<WishlistResponseDto>(wishlist);
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
}
