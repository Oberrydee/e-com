using ECommerce.API.Modules.Products.DTOs;
using FluentValidation;

namespace ECommerce.API.Modules.Products.Validators;

public class ProductInCartRequestValidator : AbstractValidator<ProductInCartRequestDto>
{
    public ProductInCartRequestValidator()
    {
        RuleFor(x => x.CartId)
            .GreaterThan(0).WithMessage("Cart ID must be greater than 0.");

        RuleFor(x => x.ProductId)
            .GreaterThan(0).WithMessage("Product ID must be greater than 0.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0.");
    }
}
