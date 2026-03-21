using ECommerce.API.Modules.Contact.DTOs;
using FluentValidation;

namespace ECommerce.API.Modules.Contact.Validators;

public class CreateUserContactRequestValidator : AbstractValidator<CreateUserContactRequestDto>
{
    public CreateUserContactRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email is required.");

        RuleFor(x => x.UserId)
            .GreaterThanOrEqualTo(0).WithMessage("User ID must be greater than or equal to 0.");

        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("Message is required.")
            .MaximumLength(4000).WithMessage("Message must not exceed 4000 characters.");
    }
}
