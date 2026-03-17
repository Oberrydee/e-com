using ECommerce.API.Common.Models;

namespace ECommerce.API.Modules.Products.Entities;

public class ProductInWishlist : BaseEntity
{
    public int WishlistId { get; set; }
    public Wishlist Wishlist { get; set; } = null!;
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
}
