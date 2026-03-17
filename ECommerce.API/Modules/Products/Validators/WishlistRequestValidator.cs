using ECommerce.API.Modules.Products.DTOs;
using FluentValidation;

namespace ECommerce.API.Modules.Products.Validators;

public class WishlistRequestValidator : AbstractValidator<WishlistRequestDto>
{
    public WishlistRequestValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("User ID must be greater than 0.");

        RuleFor(x => x.Label)
            .NotEmpty().WithMessage("Label is required.")
            .MaximumLength(150).WithMessage("Label must not exceed 150 characters.");
    }
}
