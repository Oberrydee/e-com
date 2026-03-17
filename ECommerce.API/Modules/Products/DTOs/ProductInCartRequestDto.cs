namespace ECommerce.API.Modules.Products.DTOs;

public class ProductInCartRequestDto
{
    public int CartId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
