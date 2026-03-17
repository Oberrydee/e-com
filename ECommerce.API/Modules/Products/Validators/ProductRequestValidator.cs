using ECommerce.API.Modules.Products.DTOs;
using FluentValidation;

namespace ECommerce.API.Modules.Products.Validators;

public class ProductRequestValidator : AbstractValidator<ProductRequestDto>
{
    private static readonly string[] AllowedInventoryStatuses = ["INSTOCK", "LOWSTOCK", "OUTOFSTOCK"];

    public ProductRequestValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required.")
            .MaximumLength(64).WithMessage("Code must not exceed 64 characters.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters.");

        RuleFor(x => x.Image)
            .NotEmpty().WithMessage("Image is required.")
            .MaximumLength(2048).WithMessage("Image must not exceed 2048 characters.");

        RuleFor(x => x.Category)
            .NotEmpty().WithMessage("Category is required.")
            .MaximumLength(100).WithMessage("Category must not exceed 100 characters.");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0).WithMessage("Price must be greater than or equal to 0.");

        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(0).WithMessage("Quantity must be greater than or equal to 0.");

        RuleFor(x => x.InternalReference)
            .NotEmpty().WithMessage("Internal reference is required.")
            .MaximumLength(128).WithMessage("Internal reference must not exceed 128 characters.");

        RuleFor(x => x.ShellId)
            .GreaterThanOrEqualTo(0).WithMessage("Shell ID must be greater than or equal to 0.");

        RuleFor(x => x.InventoryStatus)
            .NotEmpty().WithMessage("Inventory status is required.")
            .Must(status => AllowedInventoryStatuses.Contains(status.Trim().ToUpperInvariant()))
            .WithMessage("Inventory status must be one of: INSTOCK, LOWSTOCK, OUTOFSTOCK.");

        RuleFor(x => x.Rating)
            .InclusiveBetween(0, 5).WithMessage("Rating must be between 0 and 5.");
    }
}
