using ECommerce.API.Common.Enums;
using ECommerce.API.Common.Models;

namespace ECommerce.API.Modules.Products.Entities;

public class Product : BaseEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string InternalReference { get; set; } = string.Empty;
    public int ShellId { get; set; }
    public InventoryStatus InventoryStatus { get; set; } = InventoryStatus.OutOfStock;
    public double Rating { get; set; }
}
