using AutoMapper;
using ECommerce.API.Data;
using ECommerce.API.Modules.Products.DTOs;
using ECommerce.API.Modules.Products.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Modules.Products.Services;

public class CartService : ICartService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public CartService(ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<IReadOnlyCollection<ProductInCartResponseDto>> GetAllAsync()
    {
        var items = await _dbContext.ProductsInCart
            .AsNoTracking()
            .OrderByDescending(item => item.CreatedAt)
            .ToListAsync();

        return _mapper.Map<IReadOnlyCollection<ProductInCartResponseDto>>(items);
    }

    public async Task<ProductInCartResponseDto?> GetByIdAsync(int id)
    {
        var item = await _dbContext.ProductsInCart
            .AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == id);

        return item is null ? null : _mapper.Map<ProductInCartResponseDto>(item);
    }

    public async Task<ProductInCartResponseDto> CreateAsync(ProductInCartRequestDto request)
    {
        await EnsureReferencesExistAsync(request.CartId, request.ProductId);

        var itemExists = await _dbContext.ProductsInCart
            .AsNoTracking()
            .AnyAsync(item => item.CartId == request.CartId && item.ProductId == request.ProductId);

        if (itemExists)
        {
            throw new InvalidOperationException("This product is already in the cart.");
        }

        var now = DateTime.UtcNow;
        var item = _mapper.Map<ProductInCart>(request);
        item.CreatedAt = now;
        item.UpdatedAt = now;

        _dbContext.ProductsInCart.Add(item);
        await _dbContext.SaveChangesAsync();

        return _mapper.Map<ProductInCartResponseDto>(item);
    }

    public async Task<ProductInCartResponseDto?> UpdateAsync(int id, ProductInCartRequestDto request)
    {
        var item = await _dbContext.ProductsInCart.FirstOrDefaultAsync(productInCart => productInCart.Id == id);
        if (item is null)
        {
            return null;
        }

        await EnsureReferencesExistAsync(request.CartId, request.ProductId);

        var duplicateExists = await _dbContext.ProductsInCart
            .AsNoTracking()
            .AnyAsync(productInCart =>
                productInCart.Id != id
                && productInCart.CartId == request.CartId
                && productInCart.ProductId == request.ProductId);

        if (duplicateExists)
        {
            throw new InvalidOperationException("This product is already in the cart.");
        }

        _mapper.Map(request, item);
        item.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return _mapper.Map<ProductInCartResponseDto>(item);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var item = await _dbContext.ProductsInCart.FirstOrDefaultAsync(productInCart => productInCart.Id == id);
        if (item is null)
        {
            return false;
        }

        _dbContext.ProductsInCart.Remove(item);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    private async Task EnsureReferencesExistAsync(int cartId, int productId)
    {
        var cartExists = await _dbContext.Carts
            .AsNoTracking()
            .AnyAsync(cart => cart.Id == cartId);

        if (!cartExists)
        {
            throw new InvalidOperationException("Cart not found.");
        }

        var productExists = await _dbContext.Products
            .AsNoTracking()
            .AnyAsync(product => product.Id == productId);

        if (!productExists)
        {
            throw new InvalidOperationException("Product not found.");
        }
    }
}
