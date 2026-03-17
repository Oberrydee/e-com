using ECommerce.API.Common.Models;
using ECommerce.API.Modules.Auth.Entities;

namespace ECommerce.API.Modules.Products.Entities;

public class Cart : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
