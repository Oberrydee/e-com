using ECommerce.API.Common.Enums;
using ECommerce.API.Common.Models;
using ECommerce.API.Modules.Products.Entities;

namespace ECommerce.API.Modules.Auth.Entities;

public class User : BaseEntity
{
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.User;
    public bool IsActive { get; set; } = true;
    public Cart? Cart { get; set; }
    public ICollection<Wishlist> Wishlists { get; set; } = [];
}
