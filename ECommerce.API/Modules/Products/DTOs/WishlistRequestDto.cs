namespace ECommerce.API.Modules.Products.DTOs;

public class WishlistRequestDto
{
    public int UserId { get; set; }
    public string Label { get; set; } = string.Empty;
}
