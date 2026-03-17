using AutoMapper;
using ECommerce.API.Common.Enums;
using ECommerce.API.Data;
using ECommerce.API.Modules.Products.DTOs;
using ECommerce.API.Modules.Products.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Modules.Products.Services;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public ProductService(ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<IReadOnlyCollection<ProductResponseDto>> GetAllAsync()
    {
        var products = await _dbContext.Products
            .AsNoTracking()
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        return _mapper.Map<IReadOnlyCollection<ProductResponseDto>>(products);
    }

    public async Task<ProductResponseDto?> GetByIdAsync(int id)
    {
        var product = await _dbContext.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);

        return product is null ? null : _mapper.Map<ProductResponseDto>(product);
    }

    public async Task<ProductResponseDto> CreateAsync(ProductRequestDto request)
    {
        var product = _mapper.Map<Product>(request);
        product.InventoryStatus = ParseInventoryStatus(request.InventoryStatus);
        product.CreatedAt = DateTime.UtcNow;
        product.UpdatedAt = DateTime.UtcNow;

        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();

        return _mapper.Map<ProductResponseDto>(product);
    }

    public async Task<ProductResponseDto?> UpdateAsync(int id, ProductRequestDto request)
    {
        var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (product is null)
        {
            return null;
        }

        _mapper.Map(request, product);
        product.InventoryStatus = ParseInventoryStatus(request.InventoryStatus);
        product.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return _mapper.Map<ProductResponseDto>(product);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (product is null)
        {
            return false;
        }

        _dbContext.Products.Remove(product);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    private static InventoryStatus ParseInventoryStatus(string inventoryStatus) =>
        inventoryStatus.Trim().ToUpperInvariant() switch
        {
            "INSTOCK" => InventoryStatus.InStock,
            "LOWSTOCK" => InventoryStatus.LowStock,
            "OUTOFSTOCK" => InventoryStatus.OutOfStock,
            _ => throw new InvalidOperationException("Inventory status is invalid.")
        };
}
