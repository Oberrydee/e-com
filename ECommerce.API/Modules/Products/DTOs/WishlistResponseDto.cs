namespace ECommerce.API.Modules.Products.DTOs;

public class WishlistResponseDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Label { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
