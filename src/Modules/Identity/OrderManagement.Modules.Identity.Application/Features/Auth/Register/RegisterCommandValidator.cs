using FluentValidation;

namespace OrderManagement.Modules.Identity.Application.Features.Auth.Register;

internal sealed class RegisterCommandValidator 
    : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Email is not valid.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .MinimumLength(8)
            .WithMessage("Password must be at least 8 characters long.");
    }
}
