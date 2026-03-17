using ECommerce.API.Common.Models;
using ECommerce.API.Modules.Auth.Entities;

namespace ECommerce.API.Modules.Products.Entities;

public class Wishlist : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public string Label { get; set; } = string.Empty;
    public ICollection<ProductInWishlist> ProductsInWishlist { get; set; } = [];
}
