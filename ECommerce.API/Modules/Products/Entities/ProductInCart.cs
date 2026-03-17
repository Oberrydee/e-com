using ECommerce.API.Common.Models;

namespace ECommerce.API.Modules.Products.Entities;

public class ProductInCart : BaseEntity
{
    public int CartId { get; set; }
    public Cart Cart { get; set; } = null!;
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public int Quantity { get; set; }
}
