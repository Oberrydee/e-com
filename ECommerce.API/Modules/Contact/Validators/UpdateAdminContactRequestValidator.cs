using ECommerce.API.Modules.Contact.DTOs;
using FluentValidation;

namespace ECommerce.API.Modules.Contact.Validators;

public class UpdateAdminContactRequestValidator : AbstractValidator<UpdateAdminContactRequestDto>
{
    public UpdateAdminContactRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email is required.");
    }
}
