using AutoMapper;
using ECommerce.API.Common.Enums;
using ECommerce.API.Data;
using ECommerce.API.Modules.Products.DTOs;
using ECommerce.API.Modules.Products.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Modules.Products.Services;

public class ProductService : IProductService
{
    private const string ProductImagesDirectoryName = "storage/product-images";
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly string _contentRoot;
    private readonly string _storageRoot;

    public ProductService(ApplicationDbContext dbContext, IMapper mapper, IWebHostEnvironment webHostEnvironment)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _contentRoot = webHostEnvironment.ContentRootPath;
        _storageRoot = Path.Combine(_contentRoot, ProductImagesDirectoryName.Replace('/', Path.DirectorySeparatorChar));
        Directory.CreateDirectory(_storageRoot);
    }

    public async Task<IReadOnlyCollection<ProductResponseDto>> GetAllAsync()
    {
        var products = await _dbContext.Products
            .Include(p => p.File)
            .AsNoTracking()
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        return _mapper.Map<IReadOnlyCollection<ProductResponseDto>>(products);
    }

    public async Task<ProductResponseDto?> GetByIdAsync(int id)
    {
        var product = await _dbContext.Products
            .Include(p => p.File)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);

        return product is null ? null : _mapper.Map<ProductResponseDto>(product);
    }

    public async Task<ProductResponseDto> CreateAsync(ProductRequestDto request)
    {
        var product = _mapper.Map<Product>(request);
        var now = DateTime.UtcNow;
        product.InventoryStatus = ParseInventoryStatus(request.InventoryStatus);
        product.CreatedAt = now;
        product.UpdatedAt = now;

        var storedFile = await SaveImageAsync(request.ImageFile!);
        product.File = new ProductFile
        {
            FileName = storedFile.FileName,
            OriginalFileName = storedFile.OriginalFileName,
            ContentType = storedFile.ContentType,
            FilePath = storedFile.RelativePath,
            SizeInBytes = storedFile.SizeInBytes,
            CreatedAt = now,
            UpdatedAt = now
        };

        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();

        await _dbContext.Entry(product).Reference(p => p.File).LoadAsync();
        return _mapper.Map<ProductResponseDto>(product);
    }

    public async Task<ProductResponseDto?> UpdateAsync(int id, ProductRequestDto request)
    {
        var product = await _dbContext.Products
            .Include(p => p.File)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product is null)
        {
            return null;
        }

        var oldRelativePath = product.File?.FilePath;
        var now = DateTime.UtcNow;
        _mapper.Map(request, product);
        product.InventoryStatus = ParseInventoryStatus(request.InventoryStatus);

        var storedFile = await SaveImageAsync(request.ImageFile!);

        if (product.File is null)
        {
            product.File = new ProductFile
            {
                ProductId = product.Id,
                CreatedAt = now
            };
            _dbContext.ProductFiles.Add(product.File);
        }

        product.File.FileName = storedFile.FileName;
        product.File.OriginalFileName = storedFile.OriginalFileName;
        product.File.ContentType = storedFile.ContentType;
        product.File.FilePath = storedFile.RelativePath;
        product.File.SizeInBytes = storedFile.SizeInBytes;
        product.File.UpdatedAt = now;
        product.UpdatedAt = now;

        await _dbContext.SaveChangesAsync();

        DeleteImageIfExists(oldRelativePath, product.File.FilePath);

        return _mapper.Map<ProductResponseDto>(product);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _dbContext.Products
            .Include(p => p.File)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product is null)
        {
            return false;
        }

        var imagePathToDelete = product.File?.FilePath;
        _dbContext.Products.Remove(product);
        await _dbContext.SaveChangesAsync();

        DeleteImageIfExists(imagePathToDelete);
        return true;
    }

    private async Task<StoredFileResult> SaveImageAsync(IFormFile imageFile)
    {
        var extension = Path.GetExtension(imageFile.FileName);
        var generatedFileName = $"{Guid.NewGuid():N}{extension}";
        var absolutePath = Path.Combine(_storageRoot, generatedFileName);

        await using var stream = new FileStream(absolutePath, FileMode.Create);
        await imageFile.CopyToAsync(stream);

        return new StoredFileResult
        {
            FileName = generatedFileName,
            OriginalFileName = Path.GetFileName(imageFile.FileName),
            ContentType = imageFile.ContentType,
            RelativePath = $"{ProductImagesDirectoryName}/{generatedFileName}",
            SizeInBytes = imageFile.Length
        };
    }

    private void DeleteImageIfExists(string? relativePath, string? skipIfPathEquals = null)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
        {
            return;
        }

        if (!string.IsNullOrWhiteSpace(skipIfPathEquals)
            && string.Equals(relativePath, skipIfPathEquals, StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        var absolutePath = Path.Combine(_contentRoot, relativePath.Replace('/', Path.DirectorySeparatorChar));

        if (System.IO.File.Exists(absolutePath))
        {
            System.IO.File.Delete(absolutePath);
        }
    }

    private static InventoryStatus ParseInventoryStatus(string inventoryStatus) =>
        inventoryStatus.Trim().ToUpperInvariant() switch
        {
            "INSTOCK" => InventoryStatus.InStock,
            "LOWSTOCK" => InventoryStatus.LowStock,
            "OUTOFSTOCK" => InventoryStatus.OutOfStock,
            _ => throw new InvalidOperationException("Inventory status is invalid.")
        };

    private sealed class StoredFileResult
    {
        public string FileName { get; init; } = string.Empty;
        public string OriginalFileName { get; init; } = string.Empty;
        public string ContentType { get; init; } = string.Empty;
        public string RelativePath { get; init; } = string.Empty;
        public long SizeInBytes { get; init; }
    }
}
