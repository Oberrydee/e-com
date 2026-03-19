namespace ECommerce.API.Modules.Products.DTOs;

public class ProductResponseDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public string ImageFileName { get; set; } = string.Empty;
    public string ImageContentType { get; set; } = string.Empty;
    public long ImageSizeInBytes { get; set; }
    public string Category { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string InternalReference { get; set; } = string.Empty;
    public int ShellId { get; set; }
    public string InventoryStatus { get; set; } = string.Empty;
    public double Rating { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
