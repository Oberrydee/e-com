using Microsoft.AspNetCore.Http;

namespace ECommerce.API.Modules.Products.DTOs;

public class ProductRequestDto
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public IFormFile? ImageFile { get; set; }
    public string Category { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string InternalReference { get; set; } = string.Empty;
    public int ShellId { get; set; }
    public string InventoryStatus { get; set; } = string.Empty;
    public double Rating { get; set; }
}
